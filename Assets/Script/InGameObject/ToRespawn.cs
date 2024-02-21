using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class ToRespawn : MonoBehaviour
{
    [Header("Insert This Script to void object")]
    [SerializeField] Vector2 RespawnPoint;
    [Header("SelectTargetToRespawn")]
    [SerializeField] Transform targetToRespawn;

    //wait until do in scene
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Respawn(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Respawn(collision.gameObject);
    }

    void Respawn(GameObject collision)
    {
        if (collision.CompareTag("PlayerGame"))
        {
            targetToRespawn.gameObject.SetActive(false);
            targetToRespawn.position = RespawnPoint;
            targetToRespawn.gameObject.SetActive(true);
        }
    }
}
