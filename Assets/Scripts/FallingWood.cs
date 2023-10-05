using UnityEngine;

public class FallingWood : MonoBehaviour
{
    public Vector2 directionToPlayer;
    public bool awareOfPlayer;

    private Rigidbody2D _rigidbody2D;
    
    [SerializeField] private float playerAwarenessDistance = 10f;
    [SerializeField] private Transform player;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (IsPlayerClose())
        {
            _rigidbody2D.gravityScale = 1;
        }
    }

    private bool IsPlayerClose()
    {
        Vector2 enemyToPlayerVector = player.position - transform.position;
        directionToPlayer = enemyToPlayerVector.normalized;
        awareOfPlayer = enemyToPlayerVector.magnitude <= playerAwarenessDistance;
        return awareOfPlayer;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
