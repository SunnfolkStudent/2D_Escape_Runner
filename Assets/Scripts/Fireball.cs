using System;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float flightTime = 10f;
    [SerializeField] private float speed = 10f;
    private Animator _animator;
    [SerializeField] private float verticalVelocity;
    public float startTime = 1f;
    private Rigidbody2D _rigidbody2D;
    
    
    private void Start()
    { 
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        InvokeRepeating(nameof(FireballJump), startTime, flightTime);
        _animator = GetComponent<Animator>();
    }

    private void FireballJump()
    {
        var rotation = transform.rotation.eulerAngles.z + 90;
                var angleInRadians = rotation * Mathf.Deg2Rad;
                var playerMoveDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
                playerMoveDirection.Normalize();
                _rigidbody2D.velocity = playerMoveDirection * speed;
    }
    
   

    private void Update()
    {
        verticalVelocity = _rigidbody2D.velocity.y;
        
        if (verticalVelocity > 0) ;
        {
            _animator.SetBool("Jumping", true);
            _animator.SetBool("Falling", false);
        }
        
        if (verticalVelocity < 0)
        {
            _animator.SetBool("Falling", true);
            _animator.SetBool("Jumping", false);
        }
        
    }
}
