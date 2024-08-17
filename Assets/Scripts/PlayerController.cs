using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D col;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float hInput;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpPower;
    public float size;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        size = 1;
    }

    private bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f*size, groundLayer);
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void ApplyMovement()
    {
        rb.velocity = new Vector2(hInput * maxSpeed, rb.velocity.y);
    }

    public void GetJumpInput(InputAction.CallbackContext context)
    {
        //Debug.Log(context.performed + ", " + context.canceled);
        if(context.performed && CheckGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if(context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.3f);
        }
    }

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        hInput = context.ReadValue<Vector2>().x;
    }

    public void GetSizeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (context.ReadValue<float>() > 0)
            {
                size *= 2;
                transform.localScale *= 2 ;
            }
            else
            {
                size /= 2;
                transform.localScale /= 2;
            }
                
        }
    }
}
