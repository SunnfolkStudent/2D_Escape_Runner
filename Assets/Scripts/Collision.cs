using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    public float speedToDestroyGameObject = 10;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FireWall")
        {
            Debug.Log("FireWall hit!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}