using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    private bool _projectilePrefabIsNotNull;

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _projectilePrefabIsNotNull = fireballPrefab != null;
        SpawnFireball();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void SpawnFireball()
    {
        if (_projectilePrefabIsNotNull)
        {
            var position = gameObject.transform.position;
            var spawnPosition = new Vector3(position.x, position.y, 5);
            var spawnRotation = gameObject.transform.rotation;
            Instantiate(fireballPrefab, spawnPosition, spawnRotation);
        }
        else
        {
            Debug.LogError("Projectile prefab not assigned in the inspector.");
        }
    }
}
