﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool icyfloor = false;
    public int behaviorValue = 1;
    public float speed = 2.5f;
    public float icySpeed = 3f;
    Transform playerTransform;
    public float RetreatDistance;
    public float stopDistance;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (behaviorValue)
        {
            case 2:
                DistanceFollow();
                break;
            case 1:
                pursue();
                break;
            default:

                break;
        }
    }


    void pursue()
    {
        if (icyfloor)
        {
            rb2d.drag = 0.5f;
            Vector2 PlayerMove = playerTransform.position - transform.position;
            rb2d.AddForce(PlayerMove.normalized * icySpeed);
        }
        else
        {
            rb2d.drag = 0f; ;
            Vector2 PlayerMove = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            rb2d.MovePosition(PlayerMove);
        }
    }

    void DistanceFollow()
    {
        if(icyfloor)
        {
            rb2d.drag = 0.5f;
            if (Vector2.Distance(transform.position, playerTransform.position) > stopDistance)
            {
                Vector2 PlayerMove = playerTransform.position - transform.position;
                rb2d.AddForce(PlayerMove.normalized * icySpeed);
            }
            else if (Vector2.Distance(transform.position, playerTransform.position) < stopDistance && Vector2.Distance(transform.position, playerTransform.position) > RetreatDistance)
            {
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, playerTransform.position) < RetreatDistance)
            {
                Vector2 PlayerMove = transform.position - playerTransform.position;
                rb2d.AddForce(PlayerMove.normalized * icySpeed);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, playerTransform.position) > stopDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

            }
            else if (Vector2.Distance(transform.position, playerTransform.position) < stopDistance && Vector2.Distance(transform.position, playerTransform.position) > RetreatDistance)
            {
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, playerTransform.position) < RetreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, -speed * Time.deltaTime);
            }
        }
    }
}
