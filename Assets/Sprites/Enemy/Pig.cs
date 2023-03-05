using System;
using UnityEngine;

public class Pig : Enemy
{
    [SerializeField] private float attackRange;

    [Header("Patrol")]
    [SerializeField] private Transform leftPatrolPosition;
    [SerializeField] private Transform rightPatrolPosition;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float angryRange;
    
    private bool _isAngry;
    private bool _isInRangeAttack;
    private float timer = 0;

    [SerializeField] private LayerMask playerLayerMask;

    public override void Start()
    {
        base.Start();
        directionX = -moveSpeed;
    }
    
    public override void Update()
    {
        base.Update();
        
        if(directionX != 0) Flip(directionX);

        _isAngry = IsAngry();
        animator.SetBool("isAngry", _isAngry);

        if (!_isAngry)
        {
            directionX = Patrol(leftPatrolPosition, rightPatrolPosition, directionX);
        }
        else
        {
            _isInRangeAttack = IsTargetInAttackRange();
            
            if (!_isInRangeAttack)
            {
                timer += Time.deltaTime;
                if (timer > 0.5f)
                {
                    directionX = Follow(targetPosition, directionX);
                    timer = 0;
                }
            }
            else
            {
                Attack();
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.velocity = new Vector2(directionX, rb.velocity.y);
    }
    
    // patrol around left and right patrol position
    #region Patrol

    public float Patrol(Transform leftPosition, Transform rightPosition, float currentDirectionX)
    {
        float directionX = Mathf.Sign(currentDirectionX) * moveSpeed;

        var currentPositionX = transform.position.x;

        if (leftPosition.position.x > currentPositionX)
        {
            directionX = moveSpeed;
        }
        else if (rightPosition.position.x < currentPositionX)
        {
            directionX = -moveSpeed;
        }

        return directionX;
    }

    #endregion
    

    // angry when see player
    #region Angry
    
    private Collider2D GetTargetInAngryRange()
    {
        return Physics2D.OverlapCircle(transform.position, angryRange, playerLayerMask);
    }

    private bool IsTargetInAngryRange(Collider2D target, bool currentAngryState)
    {
        if (target != null)
        {
            targetPosition = target.transform;
            float targetPositionX = targetPosition.position.x;
            
            // target in sight
            if ((targetPositionX - transform.position.x) * directionX > 0)
            {
                if (targetPositionX >= leftPatrolPosition.position.x - 0.5f &&
                    targetPositionX <= rightPatrolPosition.position.x + 0.5f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        return currentAngryState;
    }

    private bool IsAngry()
    {
        Collider2D target = GetTargetInAngryRange();
        return IsTargetInAngryRange(target, _isAngry);
    }

    #endregion
    
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, angryRange);
        Gizmos.DrawWireSphere(position, attackRange);
    }

    #region Attack

    private bool IsTargetInAttackRange()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, attackRange, playerLayerMask);
        if (target != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public float Follow(Transform currentTargetPosition, float currentDirectionX)
    {
        float directionX = Mathf.Sign(currentDirectionX) * moveSpeed * 1.5f;

        var currentPositionX = transform.position.x;

        if (currentTargetPosition.position.x > currentPositionX)
        {
            directionX = moveSpeed * 1.5f;
        }
        else if (currentTargetPosition.position.x < currentPositionX)
        {
            directionX = -moveSpeed * 1.5f;
        }

        return directionX;
    }

    public void Attack()
    {
        //var currentPositionX = transform.position.x;
    }

    #endregion
    
    
}
