using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : KinematicObject
{
    enum EnemyState
    {
        Idle,
        Following,
        Dead
    }

    public float playerMinDistance = 3.0f;
    public float moveSpeed = 1.0f;

    private EnemyState currentState;
    private Transform playerTransform;

    private Vector2 movementVec;
    private Collider2D collider2d;
    private Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        currentState = EnemyState.Idle;
    }

    protected void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        switch(currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Following:
                break;
            case EnemyState.Dead:
                break;
        }

        base.Update();
    }

    protected override void FixedUpdate()
    {
        movementVec.y = -1f;
        int direction = SearchPlayer();

        if (isOnFixedTile)
        {
            currentState = EnemyState.Dead;
        }

        if (isOnPlayer && playerTransform != null)
        {
            PlayerController playerController = playerTransform.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Kill();
            }
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                if ( direction != 0)
                {
                    currentState = EnemyState.Following;
                }
                break;
            case EnemyState.Following:
                if (direction == 0)
                {
                    currentState = EnemyState.Idle;
                }
                movementVec.x = direction;
                break;
            case EnemyState.Dead:
                animator.SetBool("Dead", true);
                movementVec.x = 0.0f;
                break;
        }

        base.FixedUpdate();
    }

    private int SearchPlayer()
    {
        if (playerTransform == null)
        {
            return 0;
        }

        float playerDistance = Vector2.Distance(transform.position, playerTransform.position);

        if (playerDistance < playerMinDistance)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, direction );

            for(int i = 0; i < raycastHits.Length; ++i)
            {
                if (!raycastHits[i].collider.CompareTag("Enemy") && !raycastHits[i].collider.isTrigger)
                {
                    Debug.Log(raycastHits[i].collider.gameObject.name);
                    if (raycastHits[i].collider.CompareTag("Player"))
                    {
                        return (transform.position.x > playerTransform.position.x ? -1 : 1);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        return 0;
    }

    protected override void ComputeVelocity()
    {
        animator.SetFloat("VelocityX", velocity.x / moveSpeed);

        targetVelocity = movementVec * moveSpeed;
    }
}
