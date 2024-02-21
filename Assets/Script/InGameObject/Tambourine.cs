using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tambourine : InGameObject
{
    [SerializeField] float bounceForce;
    [SerializeField] Animator TamborineAnimator;
    void Start()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerGame"))
        {
            collision.gameObject.GetComponent<Player>().Bounce(bounceForce);
            if(TamborineAnimator != null)
            {
                TamborineAnimator.SetTrigger("Jump");
            }
        }
    }
}
