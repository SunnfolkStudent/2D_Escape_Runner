using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool _isCrouching;
    private bool _isSprinting;
    private int _direction = 1;
    
    private Rigidbody2D _rigidbody2D;
    private InputManager _input;
    private Animator _animator;
    
    [Header("Speeds")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float crouchSpeed = 5f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float jumpSpeed = 7f;
    
    // [SerializeField] private float staminaTime = 3f;
    // [SerializeField] private float slideDistance = 6f;
    
    [Header("CollisionBoxes")]
    [SerializeField] private GameObject collisionBox;
    [SerializeField] private GameObject crouchCollisionBox;
    [SerializeField] private GameObject slideCollisionBox;
    
    [Header("Grounded?")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float distanceToGround = 0.5f;
    [SerializeField] private bool isPlayerGrounded;
    
    [Header("Walled?")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private bool _isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;

    private bool _isWallJumping = true;
    private float _wallJumpingDirection;
    private const float WallJumpingTime = 0.2f;
    private float _wallJumpingCounter;
    private const float WallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(16f, 16f);
    
    private static readonly int Crouch = Animator.StringToHash("crouch");
    private static readonly int Sprint = Animator.StringToHash("sprint");
    private static readonly int Walk = Animator.StringToHash("walk");

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _input = GetComponent<InputManager>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
        isPlayerGrounded = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, whatIsGround);
        
        if (_input.jumpPressed && isPlayerGrounded)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed);
        }
        
        if (_input.jumpReleased && _rigidbody2D.velocity.y > 0f)
        {
            var velocity = _rigidbody2D.velocity;
            velocity = new Vector2(velocity.x, velocity.y * 0.2f );
            _rigidbody2D.velocity = velocity;
        }

        if (!_isSprinting)
        {
            if (_input.crouchPressed && isPlayerGrounded)
            {
                moveSpeed = crouchSpeed;
                collisionBox.SetActive(false);
                crouchCollisionBox.SetActive(true);
                _animator.SetBool(Crouch, true);
                _isCrouching = true;
            }

            if (_input.crouchReleased)
            {
                moveSpeed = walkSpeed;
                collisionBox.SetActive(true);
                crouchCollisionBox.SetActive(false);
                _animator.SetBool(Crouch, false);
                _isCrouching = false;
            }
        }

        if (!_isCrouching)
        {
            if (_input.sprintPressed)
            {
                _isSprinting = true;
                moveSpeed = sprintSpeed;
                _animator.SetBool(Sprint, true);
            }

            if (_input.sprintReleased)
            {
                _isSprinting = false;
                moveSpeed = walkSpeed;
                _animator.SetBool(Sprint, false);
            }
        }

        WallSlide();
        
        WallJump();
    }
    
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !isPlayerGrounded && _input.moveVector.x != 0f)
        {
            _isWallSliding = true;
            var velocity = _rigidbody2D.velocity;
            _rigidbody2D.velocity = new Vector2(velocity.x,
                Mathf.Clamp(velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void StopWallJumping()
    {
        _isWallJumping = false;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void WallJump()
    {
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpingDirection = -transform.localScale.x;
            _wallJumpingCounter = WallJumpingTime;
            
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            _wallJumpingCounter -= Time.deltaTime;
        }

        if (_input.jumpPressed && _wallJumpingCounter > 0f)
        {
            Flip();
            _isWallJumping = true;
            _rigidbody2D.velocity = new Vector2(_wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            _wallJumpingCounter = 0f;
        }
        
        Invoke(nameof(StopWallJumping), WallJumpingDuration);
    }
    
    private void FixedUpdate()
    {
        if (!_isWallJumping)
        {
            _rigidbody2D.velocity = new Vector2(_input.moveVector.x * moveSpeed, _rigidbody2D.velocity.y);
            _animator.SetBool(Walk, _input.moveVector.x != 0);
        }

        if (_input.moveVector.x < 0)
        {
             _direction = -1;
        }
        else if (_input.moveVector.x > 0)
        {
            _direction = 1;
        }

        var transform1 = transform;
        transform1.localScale = new Vector2(_direction, transform1.localScale.y);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.CompareTag("DeathTrap"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (coll.transform.CompareTag("Goal"))
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    private void Flip()
    {
        var transform1 = transform;
        var localScale = transform1.localScale;
        localScale.x *= -1f;
        transform1.localScale = localScale;
    }
}