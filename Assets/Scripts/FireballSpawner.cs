using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    private bool _projectilePrefabIsNotNull;

    private void Start()
    {
        _projectilePrefabIsNotNull = fireballPrefab != null;
        SpawnFireball();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void SpawnFireball()
    {
        if (_projectilePrefabIsNotNull)
        {
            var thisGameObject = gameObject;
            var position = thisGameObject.transform.position;
            var spawnPosition = new Vector3(position.x, position.y, 5);
            var spawnRotation = thisGameObject.transform.rotation;
            Instantiate(fireballPrefab, spawnPosition, spawnRotation);
        }
        else
        {
            Debug.LogError("Projectile prefab not assigned in the inspector.");
        }
    }
}
