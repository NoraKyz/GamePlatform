using System;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int damage;
    
    [SerializeField] protected float moveSpeed;
    protected float directionX;

    protected Animator animator;
    protected Rigidbody2D rb;
    

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }

    // default enemy move to the right
    protected virtual void Flip(float directionX)
    { 
        transform.localScale = new Vector2(Mathf.Sign(directionX), 1);
    }
}
