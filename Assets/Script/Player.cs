using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; //Player Speed when walk
    [SerializeField] float sprintAccelerate = 10f;//Player Speed when run
    [SerializeField] float jumpForce = 9.8f;
    [SerializeField] float fallMultiplier = 1f;
    [SerializeField] float stamina = 10f;

    [Header("Animation")]
    [SerializeField] Animator animator;

    [Header("FilpCamera")]
    private bool isFacingRight = true;
    private float horizontal;

    [Header("RB and GroundCheck")]
    private Rigidbody2D rb;
    public LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    
    [Header("Carry and Drop Item")]
    private bool isCarrying;
    private Item carryingItem;

    [Header("ChangeDimension")]
    bool isInSoul = true;
    bool canWarp;
    [SerializeField] PlayableDirector DimensionTransition;
    [SerializeField] GameObject[] soul, real;

    [Header("BGM")]
    public GameObject BgmSoul, BGMReal;
    [Header("Particle")]
    [SerializeField] ParticleSystem movementParticle;
    [SerializeField] ParticleSystem dropParticle;
    [Header("Sound")]
    [SerializeField] AudioSource playerSource;
    [SerializeField] AudioClip WalkOnGrass,RunOnGrass, FallOnGrass;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 45;
        soul = GameObject.FindGameObjectsWithTag("SoulDimension");
        real = GameObject.FindGameObjectsWithTag("RealDimension");
        foreach (GameObject collider in soul)
        {
            collider.SetActive(false);
            BGMReal.SetActive(true);
            BgmSoul.SetActive(false);
        }
        foreach (GameObject colliderr in real)
        {
            colliderr.SetActive(true);
        }
    }

    private void Update()
    {
        BGMReal.GetComponent<AudioSource>().volume = MenuUI.volumeSetting;
        BgmSoul.GetComponent<AudioSource>().volume = MenuUI.volumeSetting;
        if (!PlayCutscene.IsPausing)
        {
            Debug.Log(IsGrounded());
            PlayerControl();
            FlipCamera();
            PickUpOrDropItem();
            Warp();
            StartCoroutine(PlaySound());
            playerSource.volume = MenuUI.soundSetting;
        }
    }

    private void PlayerControl()
    {
        
        horizontal = Input.GetAxisRaw("Horizontal");
        if (!isCarrying)
        {
            if (Input.GetButtonDown("Jump") && IsGrounded())//Normal jump
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) //High jump
            {
                animator.SetTrigger("Jump");
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }
        //if the player falls, accelerates gravity
        if (Mathf.Abs(rb.velocity.y) > 0.5f && !IsGrounded())
        {
            if(carryingItem != null)
            {
                DropItem();
            }
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
            animator.SetBool("Fall", true);
            animator.SetBool("PickUp", false);
        }
        else
        {
            animator.SetBool("Fall", false);
        }
        FallParticleControl();
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }
    IEnumerator PlaySound()
    {
        
        playerSource.Play();
        yield return new WaitForSeconds(0.4f);
    }
    void PlayerMovement()
    {
        //when player pressed sprint
        float currentSpeed;
        if(Input.GetAxisRaw("Horizontal") != 0) 
        {
            if (Input.GetButton("Sprint") && stamina >= 1)
            {
                currentSpeed = sprintAccelerate;
                stamina -= Time.deltaTime;
                WalkParticleControl(movementParticle, 0.15f);
                playerSource.clip = RunOnGrass;
            }
            else
            {
                currentSpeed = moveSpeed;
                WalkParticleControl(movementParticle, 0.3f);
                playerSource.clip = WalkOnGrass;
            }
        }
        else
        {
            playerSource.clip = null;
            Debug.Log("IDLE");
            currentSpeed = 0f;
        }

        if (stamina <= 10)
        {
            stamina += Time.deltaTime * 0.5f;
        }
        rb.velocity = new Vector2(horizontal * currentSpeed, rb.velocity.y);
        animator.SetFloat("Speed", currentSpeed);

    }
    private void FlipCamera()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    void Warp()
    {
        if (canWarp && Input.GetKeyDown(KeyCode.F))
        {
            if (isInSoul)
            {
                StartCoroutine(ChangeDimension(real, soul,BGMReal,BgmSoul));
                isInSoul = false;
                Debug.Log("Soul");

            }
            else
            {
                StartCoroutine(ChangeDimension(soul, real, BgmSoul, BGMReal));
                isInSoul = true;
                Debug.Log("Real");
            }
            Debug.Log("WorldChange");
        }

    }
    IEnumerator ChangeDimension(GameObject[] disactive, GameObject[] active,GameObject sound1,GameObject sound2)
    {
        foreach (GameObject collider in disactive)
        {
            collider.SetActive(false);
            sound1.SetActive(false);
        }
        DimensionTransition.Play();
        yield return new WaitForSeconds(2f);
        foreach (GameObject colliderr in active)
        {
            colliderr.SetActive(true);
            sound2.SetActive(true);
        }
    }
    public void Bounce(float bounceForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, bounceForce);
    }
    private void PickUpOrDropItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("GetKey");
            if (!isCarrying)
            {
                
                TryPickupItem();
            }
            else
            {
                DropItem();
                animator.SetBool("PickUp", false);
            }
        }
    }
    IEnumerator WaitForPickUpAnimation(Collider2D collider)
    {
        Debug.Log("Scan and found item");
        animator.SetBool("PickUp", true);
        carryingItem = new Item(collider.gameObject);
        yield return new WaitForSeconds(0.66f);
        carryingItem.GotPickUp(transform);
        isCarrying = true;
        Debug.Log("PickUp Item");
    }
    private void TryPickupItem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.6f);

        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Try Carry");
            if (collider.CompareTag("HeavyItem") || collider.CompareTag("LightItem"))
            {
                StartCoroutine(WaitForPickUpAnimation(collider));
                break;
            }
        }
    }
    private void DropItem()
    {
        Debug.Log("Drop Item");
        carryingItem.GotDrop();
        carryingItem = null;
        isCarrying = false;
    }
    bool previousStateIsGround;
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    float counter;
    void WalkParticleControl(ParticleSystem particle,float particleFormation)
    {
        counter += Time.deltaTime;
        if (counter > particleFormation && IsGrounded())
        {
            particle.Play();
            counter = 0;
        }
        
    }
    private void FallParticleControl()
    {
        if (IsGrounded() && !previousStateIsGround)
        {
            // Perform some action when it becomes true again
            StartCoroutine(WaitOForDroparticlePlay());
        }
        
        // Update the previous state
        previousStateIsGround = IsGrounded();
    }
    IEnumerator WaitOForDroparticlePlay()
    {
        dropParticle.Play();
        playerSource.clip = FallOnGrass;
        Debug.Log("DropParticlePlay");
        yield return new WaitForSeconds(1.5f);
        dropParticle.Stop();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Reflection")
        {
            canWarp = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Reflection")
        {
            canWarp = false;
        }
    }
}