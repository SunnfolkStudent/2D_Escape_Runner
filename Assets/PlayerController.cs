using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpSpeed = 7f;

    public bool isPlayerGrounded;
    public LayerMask whatIsGround;
    
    private Rigidbody2D _rigidbody2D;
    private InputManager _input;
    //private Animator _animator;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _input = GetComponent<InputManager>();
        //_animator = GetComponent<Animator>();

    }

    private void Update()
    {
        Attack();
        isPlayerGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.01f, whatIsGround);
        
        if (_input.jumpPressed && isPlayerGrounded)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed);
        }
        
        if (_input.jumpReleased && _rigidbody2D.velocity.y > 0f)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * 0.2f );
        }
    }
    
    private void FixedUpdate()
    {
        _rigidbody2D.velocity = new Vector2(_input.moveVector.x * moveSpeed, _rigidbody2D.velocity.y);
    }

    private void Attack()
    {
        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), 
            new Vector2(1f, 1f), 1f, Vector2.down, 0.55f);
        if (hit.collider == null) return;
        if (!hit.transform.CompareTag("Enemy")) return;
        
        Destroy(hit.transform.gameObject);
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed / 2);
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
        
        if (coll.transform.CompareTag("RemovePlayer"))
        {
            Destroy(transform.gameObject);
        }
    }
}