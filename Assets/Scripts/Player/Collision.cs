using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Collision : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("FireWall"))
            {
                Debug.Log("Player hit FireWall!");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (collision.gameObject.CompareTag("DeathTrap"))
            {
                Debug.Log("Player hit DeathTrap!");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (collision.gameObject.CompareTag("Goal"))
            {
                SceneManager.LoadScene("WinScreen");
            }
        }
    }
}