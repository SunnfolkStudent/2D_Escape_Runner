using UnityEngine;

public class FireWallController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    public float fireWallSpeed = 5f;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocityX = fireWallSpeed;
    }
}
