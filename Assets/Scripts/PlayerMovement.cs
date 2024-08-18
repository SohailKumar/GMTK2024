using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    private bool isGrounded;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float followSharpness;
    [SerializeField] float deceleration;
    [SerializeField] float acceleration;
    private float availJumps;
    [SerializeField] float totalJumps;


    //scale vars
    public PlayerAbilityManager.PlayerSize size;
    private float numericalSize;
    private Vector3 ogScale;

    void Awake()
    {
        numericalSize = 1;
        isGrounded = false;
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
        // landed on ground
        if (!isGrounded && checkGround)
        {
            isGrounded = true;
            availJumps = totalJumps;
        }
        // left the ground
        else if (isGrounded && !checkGround)
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        //Gizmos.DrawSphere(groundCheck.position, 0.2f * numericalSize);
        Gizmos.DrawCube(groundCheck.position, new Vector3(0.7f * numericalSize, 0.2f, 1f));
    }

    private bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f * numericalSize, groundLayer);

        //return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.7f * numericalSize, 0.2f), groundLayer);
    }

    void ApplyMovement()
    {
        float blend = 1f - Mathf.Pow(1f - followSharpness, Time.deltaTime * 30f);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, transform.position + cameraOffset, blend);

        //rb.velocity = new Vector2(hInput * maxSpeed, rb.velocity.y);
        if (hInput == 0)
        {
            rb.velocity = new Vector2( Mathf.MoveTowards(rb.velocity.x, 0, deceleration * Time.fixedDeltaTime), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2( Mathf.MoveTowards(rb.velocity.x, hInput * maxSpeed, acceleration * Time.fixedDeltaTime), rb.velocity.y);
        }
    }

    public void SetSize(PlayerAbilityManager.PlayerSize size)
    {
        switch (size)
        {
            case PlayerAbilityManager.PlayerSize.Mini:
                transform.localScale = ogScale * 0.5f;
                numericalSize = 0.5f;
                mainCamera.GetComponent<Camera>().orthographicSize = 5*numericalSize;
                jumpPower = 10;
                totalJumps = 2;
                break;
            case PlayerAbilityManager.PlayerSize.Normal:
                transform.localScale = ogScale;
                numericalSize = 1f;
                mainCamera.GetComponent<Camera>().orthographicSize = 5*numericalSize;
                jumpPower = 17;
                totalJumps = 1;
                break;
            case PlayerAbilityManager.PlayerSize.Big:
                transform.localScale = ogScale * 2f;
                numericalSize = 2f;
                mainCamera.GetComponent<Camera>().orthographicSize = 5*numericalSize;
                jumpPower = 24;
                totalJumps = 1;
                break;
        }
    }

    public void GetJumpInput(InputAction.CallbackContext context)
    {
        //Debug.Log(context.performed + ", " + context.canceled);
        if(context.performed && availJumps>0)
        {
            availJumps--;
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
