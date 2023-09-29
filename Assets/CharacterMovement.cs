using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");
        var moveDirection = new Vector2(moveX, moveY);
        _rigidbody2D.velocity = new Vector2(moveDirection.x * moveSpeed, _rigidbody2D.velocity.y);

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}