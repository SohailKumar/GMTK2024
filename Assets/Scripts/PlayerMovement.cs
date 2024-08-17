using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D col;
    PlayerAbilityManager abilityManager;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float hInput;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpPower;
    public PlayerAbilityManager.PlayerSize size;
    private float numericalSize;
    public Vector3 ogScale;

    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        abilityManager = GetComponent<PlayerAbilityManager>();
        size = PlayerAbilityManager.PlayerSize.Normal;
        ogScale = transform.localScale;
    }

    private bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f*numericalSize, groundLayer);
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void ApplyMovement()
    {
        rb.velocity = new Vector2(hInput * maxSpeed, rb.velocity.y);
    }

    public void SetSize(PlayerAbilityManager.PlayerSize size)
    {
        switch (size)
        {
            case PlayerAbilityManager.PlayerSize.Mini:
                transform.localScale = ogScale * 0.5f;
                numericalSize = 0.5f;
                break;
            case PlayerAbilityManager.PlayerSize.Normal:
                transform.localScale = ogScale;
                numericalSize = 1f;
                break;
            case PlayerAbilityManager.PlayerSize.Big:
                transform.localScale = ogScale * 2f;
                numericalSize = 2f;
                break;
        }
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
                SetSize(PlayerAbilityManager.PlayerSize.Big);
            }
            else
            {
                SetSize(PlayerAbilityManager.PlayerSize.Mini);
            }
                
        }
    }

}
