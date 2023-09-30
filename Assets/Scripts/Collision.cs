using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;

    private bool _isWallSliding;
    private float _wallSlidingSpeed = 2f;

    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        WallSlide();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FireWall"))
        {
            Debug.Log("Player hit FireWall!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled())// && !isPlayerGrounded && moveVector.x != 0f
        {
            Debug.Log("wall");
            _isWallSliding = true;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,
                Mathf.Clamp(_rigidbody2D.velocity.y, -_wallSlidingSpeed, float.MaxValue));

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                _rigidbody2D.velocity = new Vector2(-10, 5f);
            }
        }
        else
        {
            _isWallSliding = false;
        }
    }
}