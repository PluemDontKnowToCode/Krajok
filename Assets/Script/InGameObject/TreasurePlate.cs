using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasurePlate : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] string triggerName;
    [SerializeField] Animator TreasurePlateAnimator,DoorAnimation;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody2D>() != null) 
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>().mass >= 50 && collision.gameObject.tag == "HeavyItem")
            {
                StartCoroutine(WaitTime(collision));
            }
        }
    }
    IEnumerator WaitTime(Collision2D collision)
    {
        door.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        Destroy(collision.gameObject);
    }
}
