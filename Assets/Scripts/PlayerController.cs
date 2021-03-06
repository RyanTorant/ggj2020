﻿using System.Collections;
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
    public float onGrabScaleMod = 0.35f;
    public bool IsGrabbing { get; private set; } = false;
    public bool IsDead { get; private set; } = false;
    public float tileGrabbingDist;
    public float flipGrabOffset = 4.0f;
    GameObject horn;

    // Internal state
    private Collider2D collider2d;
    public GameObject hornRight, hornLeft;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 movementVec;
    public JumpState jumpState = JumpState.Grounded;
    private GameObject tileToGrab;
    private BoxCollider2D tempCollider;

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }


    protected override void FixedUpdate()
    {
        if (!IsGrabbing)
        {
            tileToGrab = null;
            if (spriteRenderer.flipX)
            {
                horn = hornLeft;
            }
            else
            {
                horn = hornRight;
            }

            Vector2 hornPos = new Vector2(horn.transform.position.x, horn.transform.position.y);
            Collider2D[] hornCollisionRes = Physics2D.OverlapCircleAll(hornPos, tileGrabbingDist);

            GameObject touchingTile = null;
            foreach (Collider2D res in hornCollisionRes)
            {
                if (res.gameObject.CompareTag("DynamicTile"))
                {
                    touchingTile = res.gameObject;
                }
            }
            tileToGrab = touchingTile;
            hornCollisionRes = null;
        }

        base.FixedUpdate();
    }

    protected override void Update()
    {
        movementVec.x = Input.GetAxis("Horizontal");

        if (jumpState == JumpState.Grounded && !IsGrabbing && Input.GetButtonDown("Jump"))
            jumpState = JumpState.PrepareToJump;

        if (Input.GetButtonDown("Grab"))
        {
            if (!IsGrabbing && tileToGrab != null)
            {
                IsGrabbing = true;
                DynamicTile dynamicTileFix = tileToGrab.GetComponent<DynamicTile>();
                if (dynamicTileFix != null)
                {
                    dynamicTileFix.IsBeingGrabbed = true;
                }

                tileToGrab.transform.SetParent(horn.transform);
                tileToGrab.transform.localPosition = new Vector3((spriteRenderer.flipX ? -flipGrabOffset : 0.9f) * 0.1f, -0.05f);
                tileToGrab.transform.localScale -= new Vector3(onGrabScaleMod, onGrabScaleMod);
                var tileBody = tileToGrab.GetComponent<Rigidbody2D>();
                tileBody.bodyType = RigidbodyType2D.Kinematic;

            }
            else if (IsGrabbing && tileToGrab != null)
            {
                IsGrabbing = false;
                DynamicTile dynamicTileFix = tileToGrab.GetComponent<DynamicTile>();
                if (dynamicTileFix != null)
                {
                    dynamicTileFix.IsBeingGrabbed = false;
                }
                tileToGrab.transform.SetParent(null);
                tileToGrab.transform.localScale += new Vector3(onGrabScaleMod, onGrabScaleMod);
                var tileBody = tileToGrab.GetComponent<Rigidbody2D>();
                tileBody.bodyType = RigidbodyType2D.Dynamic;

            }
        }

        base.isGrabbing = IsGrabbing;

        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    jumpState = JumpState.InFlight;
                }
                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
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
        if (!(isOnEnemy || IsDead))
        {
            if (jumpState == JumpState.PrepareToJump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed;
                jumpState = JumpState.Jumping;
            }

            const float moveEpsilon = 0.01f;

            if (movementVec.x > moveEpsilon && !IsGrabbing && spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }
            else if (movementVec.x < -moveEpsilon && !IsGrabbing && !spriteRenderer.flipX) // Don't flip when grabbing
            {
                spriteRenderer.flipX = true;

            }
        }

        animator.SetBool("Jumping", jumpState == JumpState.Jumping || jumpState == JumpState.InFlight);
        animator.SetBool("Grounded", IsGrounded);
        animator.SetBool("Grabbing", IsGrabbing);
        animator.SetBool("Dead", isOnEnemy || IsDead);
        animator.SetFloat("VelocityX", Mathf.Abs(velocity.x) / moveSpeed);

        if (!(isOnEnemy || IsDead))
        {
            targetVelocity = movementVec * moveSpeed;
        }
    }

    public void Kill()
    {
        IsDead = true;
    }
}