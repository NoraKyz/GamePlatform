using UnityEngine;
using UnityEngine.Serialization;


public interface IAddMovement
{
    public void JumpAction();
}

public class PlayerMovement : MonoBehaviour, IAddMovement
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float slideSpeed;
    [SerializeField] private LayerMask terrainLayerMask;

    private Rigidbody2D _rb;
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;

    private Vector2 _direction;
    private int _countJump;
    private bool _isGrounded;
    private bool _isWallSliding;
    private bool _isTouchingWall;

    private static readonly int IsDoubleJump = Animator.StringToHash("isDoubleJump");
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsFall = Animator.StringToHash("isFall");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _isGrounded = IsGrounded();
        _isTouchingWall = IsTouchingWall();
        _isWallSliding = IsWallSiding();

        Move();
        Jump();
        WallSiding();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_direction.x, _rb.velocity.y);
    }

    private void Move()
    {
        _direction = Vector2.right * (Input.GetAxisRaw("Horizontal") * moveSpeed);

        if (_isGrounded)
        {
            _animator.Play("playerIdle");
            _animator.SetBool(IsRun, _direction.x != 0);

            _animator.SetBool(IsDoubleJump, false);

            _countJump = 0;
        }

        if (_direction.x != 0) Flip(_direction.x);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _countJump != 2)
        {
            _countJump++;

            // play double jump animation
            if (_countJump == 2) _animator.SetBool(IsDoubleJump, true);

            SoundController.Instance.PlaySound("jump1");

            _animator.Play("playerJump");

            _rb.velocity = Vector2.zero;
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //check if player is jumping or falling to play animation
        _animator.SetBool(IsFall, _rb.velocity.y >= 0);
    }

    private void WallSiding()
    {
        if (_isWallSliding && _rb.velocity.y < 0)
        {
            _countJump = 1;

            Vector2 normal = transform.position.normalized;
            Vector2 slideDirection = Vector2.Reflect(_rb.velocity, normal);
            _rb.velocity = slideDirection * slideSpeed;

            _animator.Play("playerWallJump");
        }
    }

    // check if player is on the ground
    private bool IsGrounded()
    {
        var bounds = _boxCollider2D.bounds;
        RaycastHit2D raycastHit = Physics2D.BoxCast(bounds.center, bounds.size,
            0, Vector2.down, 0.1f, terrainLayerMask);
        return raycastHit.collider != null;
    }

    private bool IsTouchingWall()
    {
        // check if the player is touching the wall
        Vector2 currentPlayerPosition = transform.position;
        var lenghtOfRaycast = _boxCollider2D.bounds.size.x / 2 + 0.025f;
        var leftRaycast = Physics2D.Raycast(currentPlayerPosition, -Vector2.right, lenghtOfRaycast, terrainLayerMask);
        var rightRaycast = Physics2D.Raycast(currentPlayerPosition, Vector2.right, lenghtOfRaycast, terrainLayerMask);

        Debug.DrawRay(currentPlayerPosition, Vector2.right * lenghtOfRaycast, Color.blue);
        Debug.DrawRay(currentPlayerPosition, -Vector2.right * lenghtOfRaycast, Color.blue);

        return (leftRaycast.collider != null || rightRaycast.collider != null);
    }

    private bool IsWallSiding()
    {
        return (_isGrounded == false && _isTouchingWall);
    }

    private void Flip(float directionX)
    {
        transform.localScale = new Vector2(Mathf.Sign(directionX), 1);
    }


    public void JumpAction()
    {
        _countJump = 1;

        SoundController.Instance.PlaySound("jump1");

        _animator.Play("playerJump");

        _rb.velocity = Vector2.zero;
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}