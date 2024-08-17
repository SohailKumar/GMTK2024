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
    Camera mainCamera;
    Vector3 cameraOffset;

    //movement vars
    private float hInput;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpPower;

    //scale vars
    public PlayerAbilityManager.PlayerSize size;
    private float numericalSize;
    private Vector3 ogScale;

    void Awake()
    {
        numericalSize = 1;
        Physics2D.queriesStartInColliders = false;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        abilityManager = GetComponent<PlayerAbilityManager>();
        size = PlayerAbilityManager.PlayerSize.Normal;
        ogScale = transform.localScale;
        mainCamera = Camera.main;
        cameraOffset = mainCamera.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        CheckCollisions();
        ApplyMovement();
    }

    private void CheckCollisions()
    {
        bool checkGround = CheckGrounded();
    }

    private bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f * numericalSize, groundLayer);
    }

    void ApplyMovement()
    {
        mainCamera.transform.position = transform.position + cameraOffset;

        rb.velocity = new Vector2(hInput * maxSpeed, rb.velocity.y);
        //if (hInput == 0)
        //{
        //    var deceleration = grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
        //    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        //}
        //else
        //{
        //    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
        //}
    }

    public void SetSize(PlayerAbilityManager.PlayerSize size)
    {
        switch (size)
        {
            case PlayerAbilityManager.PlayerSize.Mini:
                transform.localScale = ogScale * 0.5f;
                numericalSize = 0.5f;
                mainCamera.GetComponent<Camera>().orthographicSize = 5*numericalSize;
                break;
            case PlayerAbilityManager.PlayerSize.Normal:
                transform.localScale = ogScale;
                numericalSize = 1f;
                mainCamera.GetComponent<Camera>().orthographicSize = 5*numericalSize;
                break;
            case PlayerAbilityManager.PlayerSize.Big:
                transform.localScale = ogScale * 2f;
                numericalSize = 2f;
                mainCamera.GetComponent<Camera>().orthographicSize = 5*numericalSize;
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
