using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
public class Falling : InGameObject
{
    [SerializeField] float fallDelay = 2.0f;  // Time in seconds before the platform falls
    [SerializeField] float respawnDelay = 5.0f; // Time in seconds before the platform respawns
    private Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        base.Awake();
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        initialPosition = transform.position;
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerGame"))
        {
            Invoke("Fall", fallDelay);
        }
    }
    void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // Change to dynamic to make it fall.
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        Invoke("Respawn", respawnDelay);
    }

    void Respawn()
    {
        rb.bodyType = RigidbodyType2D.Static; // Change back to static.
        transform.position = initialPosition; // Reset the platform to its initial position.
    }
}
