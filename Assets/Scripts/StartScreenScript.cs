using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenScript : MonoBehaviour
{
    private InputManager _input;

    private void Start()
    {
        _input = GetComponent<InputManager>();
        Cursor.visible = true;
    }

    private void Update()
    {
        if (_input.jumpPressed)
        {
            Play();
        }
        // if (_input.crouchPressed)
        // {
        //     Quit();
        // }
    }

    public void Play()
    {
        SceneManager.LoadScene("Tutorial");
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void Scores()
    {
        SceneManager.LoadScene("Scores");
    }
}
