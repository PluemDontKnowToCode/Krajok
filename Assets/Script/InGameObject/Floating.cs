using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlatformEffector2D))]

public class Floating : InGameObject
{
    [SerializeField] float speed = 2.0f;
    private Vector2 targetPos;
    [SerializeField] Vector2 FirstPos, SecondPos;
    // Start is called before the first frame update
    void Start()
    {
        ForSomePlatform();
        targetPos = SecondPos;
        FirstPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Check if the target position is reached, and if so, change the target to the other position
        if ((Vector2)transform.position == SecondPos)
        {
            targetPos = FirstPos;
        }
        else if ((Vector2)transform.position == FirstPos)
        {
            targetPos = SecondPos;
        }
    }
}
