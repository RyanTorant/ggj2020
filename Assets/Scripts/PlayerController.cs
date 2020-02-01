using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : KinematicObject
{
    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }

    // Public interface
    public float moveSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    // Internal state
    private Collider2D collider2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 movementVec;
    private JumpState jumpState = JumpState.Grounded;

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        movementVec.x = Input.GetAxis("Horizontal");

        if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
            jumpState = JumpState.PrepareToJump;

        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    //Schedule<PlayerJumped>().player = this;
                    jumpState = JumpState.InFlight;
                }
                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
                    //Schedule<PlayerLanded>().player = this;
                    jumpState = JumpState.Landed;
                }
                break;
            case JumpState.Landed:
                jumpState = JumpState.Grounded;
                break;
        }

        base.Update();
    }

    protected override void ComputeVelocity()
    {
        if (jumpState == JumpState.PrepareToJump && IsGrounded)
        {
            velocity.y = jumpTakeOffSpeed;
            jumpState = JumpState.Jumping;
        }

        const float moveEpsilon = 0.01f;

        if (movementVec.x > moveEpsilon)
            spriteRenderer.flipX = false;
        else if (movementVec.x < -moveEpsilon)
            spriteRenderer.flipX = true;

        animator.SetBool("grounded", IsGrounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / moveSpeed);

        targetVelocity = movementVec * moveSpeed;
    }
}
