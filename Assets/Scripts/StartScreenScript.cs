using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenScript : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Tutorial");
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
