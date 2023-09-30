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
    [SerializeField] private LayerMask groundLayer;

    private bool _isWallSliding;
    private float _wallSlidingSpeed = 2f;

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
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !isPlayerGrounded && _input.moveVector.x != 0f)
        {
            Debug.Log("wall");
            _isWallSliding = true;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,
                Mathf.Clamp(_rigidbody2D.velocity.y, -_wallSlidingSpeed, float.MaxValue));

            if (_input.jumpReleased)
            {
                _rigidbody2D.velocity = new Vector2(-10, 5f);
            }
        }
        else
        {
            _isWallSliding = false;
        }
    }
    
    private void FixedUpdate()
    {
        _rigidbody2D.velocity = new Vector2(_input.moveVector.x * moveSpeed, _rigidbody2D.velocity.y);
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
}