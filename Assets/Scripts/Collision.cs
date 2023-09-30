using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FireWall"))
        {
            Debug.Log("Player hit FireWall!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}