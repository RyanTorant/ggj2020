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
    public float onGrabScaleMod = 0.35f;
    public bool IsGrabbing { get; private set; } = false;
    public bool IsDead { get; private set; } = false;
    public float tileGrabbingDist;

    // Internal state
    private Collider2D collider2d;
    public GameObject horn;
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

    /*DELETE?
     * private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DynamicTile"))
        {
            tileToGrab = other.gameObject;
        }
    }*/

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == tileToGrab && !IsGrabbing)
        {
            tileToGrab = null;
        }
    }

    protected override void FixedUpdate()
    {
        Vector2 hornPos = new Vector2(horn.transform.position.x, horn.transform.position.y);
        Collider2D[] hornCollisionRes = Physics2D.OverlapCircleAll(hornPos, tileGrabbingDist);

        foreach (Collider2D res in hornCollisionRes)
        {
            if (res.gameObject.CompareTag("DynamicTile"))
            {
                tileToGrab = res.gameObject;
            }
        }

        base.FixedUpdate();
        hornCollisionRes = null;
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
                DynamicTile dynamicTileFix = tileToGrab.GetComponent<DynamicTile>();
                if (dynamicTileFix != null)
                {
                    dynamicTileFix.IsBeingGrabbed = true;
                }
                
                tileToGrab.transform.SetParent(transform);
                tileToGrab.transform.localPosition += new Vector3(0, 0.1f);
                tileToGrab.transform.localScale -= new Vector3(onGrabScaleMod, onGrabScaleMod);
                var tileBody = tileToGrab.GetComponent<Rigidbody2D>();
                tileBody.bodyType = RigidbodyType2D.Kinematic;
            }
            else if(IsGrabbing && tileToGrab != null)
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

        if (movementVec.x > moveEpsilon && !IsGrabbing && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
            if (horn.transform.position.x < 0)
            {
                horn.transform.localPosition = new Vector3(-1 * horn.transform.localPosition.x, horn.transform.localPosition.y, horn.transform.localPosition.z);
               // collider2d.offset *= new Vector2(-1, 1); 
            }

        }
        else if (movementVec.x < -moveEpsilon && !IsGrabbing && !spriteRenderer.flipX) // Don't flip when grabbing
        { 
            spriteRenderer.flipX = true;
            if (horn.transform.localPosition.x > 0)
              {
                  horn.transform.localPosition = new Vector3(-1 * horn.transform.localPosition.x, horn.transform.localPosition.y, horn.transform.localPosition.z);
                  //collider2d.offset *= new Vector2(-1, 1);
              }

        }
        animator.SetBool("Jumping", jumpState == JumpState.Jumping || jumpState == JumpState.InFlight);
        animator.SetBool("Grounded", IsGrounded);
        animator.SetBool("Grabbing", IsGrabbing);
        animator.SetBool("Dead", isOnEnemy || IsDead);
        animator.SetFloat("VelocityX", Mathf.Abs(velocity.x) / moveSpeed);

        targetVelocity = movementVec * moveSpeed;
    }

    public void Kill()
    {
        IsDead = true;
    }
}
