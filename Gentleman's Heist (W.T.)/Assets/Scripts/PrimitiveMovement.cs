using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Normal wasd/arrows but leftshift to sneak
 */
public class PrimitiveMovement : MonoBehaviour
{
    public float speed = 3f;

    public Rigidbody2D rb;

    private Vector2 _movement;

    // somewhat evil implementation of sneak to avoid if/else block
    private int _isSneak = 1;

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isSneak = 2;
        }
        else
        {
            _isSneak = 1;
        }
        rb.velocity = _movement * speed / _isSneak;
    }

    private void FixedUpdate()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
}
