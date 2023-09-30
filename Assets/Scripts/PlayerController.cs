using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float crouchVariable = 2f;
    public float sprintVariable = 2f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 7f;
    public float distanceToGround = 2f;
    public float staminaTime = 3f;
    public float slideDistance = 6f;
    public GameObject collisionBox;
    public GameObject crouchCollisionBox;

    public bool isPlayerGrounded;
    public LayerMask whatIsGround;
    
    private Rigidbody2D _rigidbody2D;
    private InputManager _input;
    //private Animator _animator;
    
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private int _direction = 1;

    private bool _isWallSliding;
    private readonly float _wallSlidingSpeed = 2f;
    
    private bool _isWallJumping = true;
    private float _wallJumpingDirection;
    private const float WallJumpingTime = 0.2f;
    private float _wallJumpingCounter;
    private const float WallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(15f, 16f);

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _input = GetComponent<InputManager>();
        //_animator = GetComponent<Animator>();

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

        if (_input.crouchPressed && isPlayerGrounded)
        {
            moveSpeed /= crouchVariable;
            
            collisionBox.SetActive(false);
            crouchCollisionBox.SetActive(true);
            
        }

        if (_input.crouchReleased)
        {
            moveSpeed *= crouchVariable;
            collisionBox.SetActive(true);
            crouchCollisionBox.SetActive(false);
        }

        if (_input.sprintPressed)
        {
            moveSpeed *= sprintVariable;
        }

        if (_input.sprintReleased)
        {
            moveSpeed /= sprintVariable;
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
                Mathf.Clamp(velocity.y, -_wallSlidingSpeed, float.MaxValue));
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
        Vector3 localScale = transform1.localScale;
        localScale.x *= -1f;
        transform1.localScale = localScale;
    }
}