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

    private int direction = 1;

    private bool _isWallSliding;
    private float _wallSlidingSpeed = 2f;

    
    private bool isFacingRight = true;
    
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
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
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.2f );
        }

        if (_input.crouchPressed && isPlayerGrounded)
        {
            moveSpeed /= crouchVariable;
            
        }

        if (_input.crouchReleased)
        {
            moveSpeed *= crouchVariable;
            
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
        
        if (!isWallJumping)
        {
            Flip();
        }
    }
    
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !isPlayerGrounded)// && _input.moveVector.x != 0f
        {
            Debug.Log("wall");
            _isWallSliding = true;
            wallJumpingDirection = transform.localScale.x;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,
                Mathf.Clamp(_rigidbody2D.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void WallJump()
    {
        if (_isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (_input.jumpPressed && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            _rigidbody2D.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
        }
        
        if (transform.localScale.x != wallJumpingDirection)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
        
        Invoke(nameof(StopWallJumping), wallJumpingDuration);
    }
    
    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            _rigidbody2D.velocity = new Vector2(_input.moveVector.x * moveSpeed, _rigidbody2D.velocity.y);
        }

        if (_input.moveVector.x < 0)
        {
             direction = -1;
        }
        else if (_input.moveVector.x > 0)
        {
            direction = 1;
        }
        
        transform.localScale = new Vector2(direction, transform.localScale.y);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.CompareTag("DeathTrap"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (coll.transform.CompareTag("Goal"))
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    private void Flip()
    {
        if (isFacingRight && _input.moveVector.x < 0f || !isFacingRight && _input.moveVector.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}