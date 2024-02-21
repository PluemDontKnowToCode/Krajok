using System.Collections;
using UnityEngine;
[RequireComponent(typeof(PlatformEffector2D))]
public class Normal : InGameObject
{
    BoxCollider2D playerCollider, groundCheck;
    private void Start()
    {
        base.Awake();
        ForSomePlatform();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (playerCollider != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerGame")
        {
            playerCollider = collision.gameObject.GetComponent<BoxCollider2D>();
            groundCheck = collision.gameObject.transform.Find("Ground Check").GetComponent<BoxCollider2D>();
        }
    }


    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        Physics2D.IgnoreCollision(groundCheck, platformCollider);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        Physics2D.IgnoreCollision(groundCheck, platformCollider,false);
    }
}
