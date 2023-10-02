using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float flightTime = 10f;
    [SerializeField] private float speed = 10f;

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        var rotation = transform.rotation.eulerAngles.z + 90;
       var angleInRadians = rotation * Mathf.Deg2Rad;
       var playerMoveDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
       playerMoveDirection.Normalize();
       _rigidbody2D.velocity = playerMoveDirection * speed;
       
       Invoke(nameof(DestroyFireball), flightTime);
    }

    private void DestroyFireball()
    {
        Destroy(gameObject);
    }
}
