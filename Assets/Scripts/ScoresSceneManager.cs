using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoresSceneManager : MonoBehaviour
{
    private InputManager _inputManager;

    private void Start()
    {
        _inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (_inputManager.pause || _inputManager.crouchPressed)
        {
            SceneManager.LoadScene("StartScreen");
        }
    }
}
