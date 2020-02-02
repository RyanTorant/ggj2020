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
    public float moveSpeed = 2.0f;
    public float jumpTakeOffSpeed = 3.5f;
    public bool IsGrabbing { get; private set; } = false;
    public bool IsDead { get; private set; } = false;

    // Internal state
    private Collider2D collider2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 movementVec;
    public JumpState jumpState = JumpState.Grounded;
    private GameObject tileToGrab;

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        AudioManager.PlayMusic(0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DynamicTile"))
        {
            tileToGrab = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == tileToGrab)
        {
            tileToGrab = null;
        }
    }

    protected override void Update()
    {
        movementVec.x = Input.GetAxis("Horizontal");

        if (jumpState == JumpState.Grounded && !IsGrabbing && Input.GetButtonDown("Jump"))
            jumpState = JumpState.PrepareToJump;

        if (Input.GetButtonDown("Grab"))
        {
            if(!IsGrabbing && tileToGrab != null)
            {
                IsGrabbing = true;
                tileToGrab.transform.SetParent(transform);
                tileToGrab.transform.localPosition += new Vector3(0, 0.1f);
                tileToGrab.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            }
            else if(IsGrabbing && tileToGrab != null)
            {
                IsGrabbing = false;
                tileToGrab.transform.SetParent(null);
            }
        }

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
        else if (movementVec.x < -moveEpsilon && !IsGrabbing) // Don't flip when grabbing
            spriteRenderer.flipX = true;

        animator.SetBool("Jumping", jumpState == JumpState.Jumping || jumpState == JumpState.InFlight);
        animator.SetBool("Grounded", IsGrounded);
        animator.SetBool("Grabbing", IsGrabbing);
        animator.SetBool("Dead", IsDead);
        animator.SetFloat("VelocityX", Mathf.Abs(velocity.x) / moveSpeed);

        targetVelocity = movementVec * moveSpeed;
    }
}
