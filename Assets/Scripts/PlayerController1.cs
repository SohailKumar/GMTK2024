using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController1 : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D col;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float Acceleration;
    [SerializeField] private float JumpPower;    
    [SerializeField] private float CollisionDetectionDistance; //The detection distance for grounding and roof detection
    [SerializeField] private float GroundDeceleration;
    [SerializeField] private float AirDeceleration;
    [SerializeField] private float GroundingForce;
    [SerializeField] private float FallAcceleration;
    [SerializeField] private float MaxFallSpeed;

    private Vector2 frameVelocity;
    public event Action<bool, float> GroundedChanged; //can be subscribed to to change animations when in the air. bool for whether in ground or air. float to determine the intensity of the landing(higher=more intense)

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        Physics2D.queriesStartInColliders = false; //CAREFUL DOING RAYCASTS WITH THIS
    }

    void FixedUpdate()
    {
        CheckCollisions();

        ApplyMovement();
    }

    private bool grounded;
    void CheckCollisions()
    {
        bool groundHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.down, CollisionDetectionDistance, gameObject.layer);
        Debug.Log("Ground Hit: "+ groundHit);
        bool ceilingHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.up, CollisionDetectionDistance, groundLayer);

        //if (ceilingHit)
        //{
        //    frameVelocity.y = Mathf.Min(0, frameVelocity.y);
        //}

        if(!grounded && groundHit) //Landed on Ground
        {
            grounded = true;
            //GroundedChanged?.Invoke(true, Mathf.Abs(frameVelocity.y));
        }
        else if(grounded && !groundHit) //Left the ground
        {
            grounded = false;
            //GroundedChanged?.Invoke(false, 0);
        }
    }


    void HandleMovement()
    {
        if (horizontal < 0)
        {
            var deceleration = grounded ? GroundDeceleration : AirDeceleration;
            frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, horizontal * MaxSpeed, Acceleration * Time.fixedDeltaTime);
        }
    }

    //void HandleJump()
    //{
    //    frameVelocity.y = JumpPower;
    //}


    void ApplyMovement()
    {
        rb.velocity = frameVelocity;
    }

    //private void HandleGravity()
    //{
    //    if (grounded && frameVelocity.y <= 0f)
    //    {
    //        frameVelocity.y = GroundingForce;
    //    }
    //    else
    //    {
    //        var inAirGravity = FallAcceleration;
    //        frameVelocity.y = Mathf.MoveTowards(frameVelocity.y, -MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
    //    }
    //}

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
}
