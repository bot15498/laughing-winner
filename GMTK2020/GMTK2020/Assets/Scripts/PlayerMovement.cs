﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool useArrowKeys = false;
    public bool canControlMove = true;
    public bool mirrored = false;
    public bool icyFloor = false;
    public bool snakeMovement = false;
    public float speed;
    public float icySpeed = 3.5f;
    public Transform CursorDirection;
    public SpriteRenderer WeaponSprite;
    bool facingRight;
    Rigidbody2D playerRigidbody;
    private SpriteRenderer sr;
    private float lastValidH = 1f;
    private float lastValidV = 1f;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        WeaponFlipping();
    }


    void Move()
    {
        float h = 0f;
        float v = 0f;
        if(!useArrowKeys)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal2");
            v = Input.GetAxisRaw("Vertical2");
        }

        if (snakeMovement && Mathf.Abs(h) < 0.01f && Mathf.Abs(v) < 0.01f)
        {
            h = lastValidH;
            v = lastValidV;
        }

        if (mirrored)
        {
            h = h * -1;
            v = v * -1;
        }
        if (canControlMove)
        {
            if (icyFloor)
            {
                playerRigidbody.drag = 0.5f;
                Vector2 PlayerMove = new Vector2(h, v);
                playerRigidbody.AddForce(PlayerMove.normalized * icySpeed);
            }
            else
            {
                playerRigidbody.velocity = new Vector2(h * speed, v * speed);
            }
        }
        lastValidH = h;
        lastValidV = v;
    }

    public void StopMoving()
    {
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.angularVelocity = 0f;
    }

    void WeaponFlipping()
    {
        //current mouse position
        var mousePos = Input.mousePosition;
        mousePos.z = 10;
        var delta = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;

        //CharacterFlipping
        if (delta.x >= 0 && !facingRight)
        { // mouse is on right side of player

            Vector3 newScale = transform.localScale;
            newScale.x = 1;
            transform.localScale = newScale;
            CursorDirection.transform.localScale = newScale;
           // sr.flipX = false;
            WeaponSprite.flipY = false;
            facingRight = true;

        }
        else if (delta.x < 0 && facingRight)
        { // mouse is on left side
            Vector3 newScale = transform.localScale;
            newScale.x = -1;
            transform.localScale = newScale;
            CursorDirection.transform.localScale = newScale;
            //sr.flipX = true;
            WeaponSprite.flipY = true;

            
            facingRight = false;
        }
    }


}
