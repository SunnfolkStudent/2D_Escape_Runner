using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool isCrouching;
        [SerializeField] private bool isSliding;
        [SerializeField] private bool isUnderGround;
        [SerializeField] private bool isCrouchedReleased;
        [SerializeField] private bool isSprinting;
        [SerializeField] private bool isWalking;
        [SerializeField] private bool isJumping;
        [SerializeField] private bool isFalling;
        [SerializeField] private bool isWallSliding;
        [SerializeField] private bool isWallJumping;
    
        private int _direction = 1;
    
        private Rigidbody2D _rigidbody2D;
        private InputManager _input;
        private Animator _animator;
    
        [Header("Speeds")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float walkSpeed = 10f;
        [SerializeField] private float fallMoveSpeed = 7.5f;
        [SerializeField] private float crouchSpeed = 5f;
        [SerializeField] private float slideSpeed = 12.5f;
        [SerializeField] private float sprintSpeed = 15f;
        [SerializeField] private float jumpSpeed = 7f;
    
        [Header("CollisionBoxes")]
        [SerializeField] private GameObject collisionBox;
        [SerializeField] private GameObject crouchCollisionBox;
        [SerializeField] private GameObject slideCollisionBox;
        [SerializeField] private float slideTime = 2f;
    
        [Header("Grounded?")]
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private float distanceToGround = 0.5f;
        [SerializeField] private bool isPlayerGrounded;
    
        [Header("Walled?")]
        [SerializeField] private Transform wallCheck;
        [SerializeField] private LayerMask wallLayer;

        [SerializeField] private float wallSlidingSpeed = 2f;
        private float _wallJumpingDirection;
        private const float WallJumpingTime = 0.2f;
        private float _wallJumpingCounter;
        private const float WallJumpingDuration = 0.4f;
        [SerializeField] private Vector2 wallJumpingPower = new Vector2(16f, 16f);

        private bool _canJump = true;
    
        private static readonly int CrouchAnimation = Animator.StringToHash("crouch");
        private static readonly int SlideAnimation = Animator.StringToHash("slide");
        private static readonly int SprintAnimation = Animator.StringToHash("sprint");
        private static readonly int WalkAnimation = Animator.StringToHash("walk");
        private static readonly int JumpAnimation = Animator.StringToHash("jump");
        private static readonly int FallingAnimation = Animator.StringToHash("falling");
        private static readonly int GroundedAnimation = Animator.StringToHash("grounded");
        private static readonly int WallJumpingAnimation = Animator.StringToHash("wallJumping");
        private static readonly int WallSlidingAnimation = Animator.StringToHash("wallSliding");

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _input = GetComponent<InputManager>();
            _animator = GetComponent<Animator>();
        
            Walk();
        }

        private void Update()
        {
            isPlayerGrounded = IsPlayerGrounded();
            isUnderGround = IsUnderGround();
        
            if (_input.crouchReleased || (isUnderGround && isCrouching)) isCrouchedReleased = true;
        
            if (isPlayerGrounded)
            {
                _animator.SetBool(FallingAnimation, false);
                isFalling = false;
            }
        
            _animator.SetBool(GroundedAnimation, isPlayerGrounded);
        
            if (_input.jumpPressed && isPlayerGrounded && _canJump)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed);
                isJumping = true;
                _animator.SetBool(JumpAnimation, true);
            }
        
            if (_input.jumpReleased && _rigidbody2D.velocity.y > 0f)
            {
                var velocity = _rigidbody2D.velocity;
                velocity = new Vector2(velocity.x, velocity.y * 0.2f );
                _rigidbody2D.velocity = velocity;
            }
        
            if (_rigidbody2D.velocity.y < 0f)
            {
                _animator.SetBool(JumpAnimation, false);
                isJumping = false;
                _animator.SetBool(FallingAnimation, true);
                isFalling = true;
            }
        
            if (_input.crouchPressed && isPlayerGrounded)
            {
                if (isSprinting)
                {
                    Slide();
                }
                else
                {
                    Crouch();
                }
            }
        
            if (isCrouchedReleased && !isUnderGround && !isSprinting)
            {
                Walk();
            }
        
            if (_input.sprintPressed && isPlayerGrounded && !isCrouching && !isUnderGround)
            {
                Sprint();
            }

            if (_input.sprintReleased && !isUnderGround)
            {
                Walk();
            }

            WallSlideCheck();
        
            WallJumpCheck();
        
            if (isUnderGround && !isCrouching && !isSliding) Crouch();

            if (isPlayerGrounded && moveSpeed <= 10 && !isCrouching || isWallSliding) moveSpeed = walkSpeed;
        }

        private bool IsPlayerGrounded()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, whatIsGround);
        }

        private bool IsUnderGround()
        {
            return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1), Vector2.up, 0.9f, whatIsGround);
        }

        private void Walk()
        {
            moveSpeed = walkSpeed;
        
            isWalking = true;
            isCrouching = false;
            isSliding = false;
            isSprinting = false;
            isCrouchedReleased = false;
            _canJump = true;
        
            collisionBox.SetActive(true);
            crouchCollisionBox.SetActive(false);
            slideCollisionBox.SetActive(false);
        
            _animator.SetBool(WalkAnimation, true);
            _animator.SetBool(CrouchAnimation, false);
            _animator.SetBool(SlideAnimation, false);
            _animator.SetBool(SprintAnimation, false);
        }

        private void Sprint()
        {
            moveSpeed = sprintSpeed;
        
            isWalking = false;
            isCrouching = false;
            isSliding = false;
            isSprinting = true;
            isCrouchedReleased = false;
            _canJump = true;
        
            collisionBox.SetActive(true);
            crouchCollisionBox.SetActive(false);
            slideCollisionBox.SetActive(false);
        
            _animator.SetBool(WalkAnimation, false);
            _animator.SetBool(CrouchAnimation, false);
            _animator.SetBool(SlideAnimation, false);
            _animator.SetBool(SprintAnimation, true);
        }

        private void Crouch()
        {
            moveSpeed = crouchSpeed;
        
            isWalking = false;
            isCrouching = true;
            isSliding = false;
            isSprinting = false;
            //isCrouchedReleased = false;
            _canJump = false;
        
            collisionBox.SetActive(false);
            crouchCollisionBox.SetActive(true);
            slideCollisionBox.SetActive(false);
        
            _animator.SetBool(WalkAnimation, false);
            _animator.SetBool(CrouchAnimation, true);
            _animator.SetBool(SlideAnimation, false);
            _animator.SetBool(SprintAnimation, false);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Slide()
        {
            moveSpeed = slideSpeed;
        
            isWalking = false;
            isCrouching = false;
            isSliding = true;
            isSprinting = false;
            //isCrouchedReleased = false;
            _canJump = false;
        
            collisionBox.SetActive(false);
            crouchCollisionBox.SetActive(false);
            slideCollisionBox.SetActive(true);
        
            _animator.SetBool(WalkAnimation, false);
            _animator.SetBool(CrouchAnimation, false);
            _animator.SetBool(SlideAnimation, true);
            _animator.SetBool(SprintAnimation, false);
        
            Invoke(IsUnderGround() ? nameof(Crouch) : nameof(Walk), slideTime);
        }
    
        private void WallSlide()
        {
            isWallSliding = true;
            var velocity = _rigidbody2D.velocity;
            _rigidbody2D.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -wallSlidingSpeed, float.MaxValue));
            _animator.SetBool(WallSlidingAnimation, true);
        }
    
        private bool IsWalled()
        {
            return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        }

        private void WallSlideCheck()
        {
            if (IsWalled() && !isPlayerGrounded && _input.moveVector.x != 0f && !isWallJumping)
            {
                WallSlide();
            }
            else
            {
                isWallSliding = false;
                _animator.SetBool(WallSlidingAnimation, false);
            }
        }

        private void StopWallJumping()
        {
            isWallJumping = false;
            _animator.SetBool(WallJumpingAnimation,false);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void WallJumpCheck()
        {
            if (isWallSliding)
            {
                isWallJumping = false;
                _animator.SetBool(WallJumpingAnimation,false);
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
                isWallJumping = true;
                _animator.SetBool(WallJumpingAnimation,true);
                _rigidbody2D.velocity = new Vector2(_wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
                _wallJumpingCounter = 0f;
            }
        
            Invoke(nameof(StopWallJumping), WallJumpingDuration);
        }
    
        private void FixedUpdate()
        {
            if (isFalling && moveSpeed > 2.5f) moveSpeed *= 0.99f;
            if (!isWallJumping)
            {
                _rigidbody2D.velocity = new Vector2( _input.moveVector.x * moveSpeed, _rigidbody2D.velocity.y);
                _animator.SetBool(WalkAnimation, _input.moveVector.x != 0);
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

        private void Flip()
        {
            var transform1 = transform;
            var localScale = transform1.localScale;
            localScale.x *= -1f;
            transform1.localScale = localScale;
        }
    }
}