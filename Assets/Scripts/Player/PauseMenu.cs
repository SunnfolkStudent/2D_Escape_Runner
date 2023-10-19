using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private InputManager input;
        public GameObject paused;
        public GameObject resume;
        public GameObject mainMenu;
        private bool _isPaused = false;

        private void Start()
        {
            paused.SetActive(false);
            resume.SetActive(false);
            mainMenu.SetActive(false);
        }

        private void Update()
        {
            if (!input.pause) return;
            if (!_isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }

        private void Pause()
        {
            _isPaused = true;
            Cursor.visible = true;
            
            paused.SetActive(true);
            resume.SetActive(true);
            mainMenu.SetActive(true);
            
            Time.timeScale = 0;
        }

        public void Unpause()
        {
            _isPaused = false;
            Cursor.visible = false;
            
            paused.SetActive(false);
            resume.SetActive(false);
            mainMenu.SetActive(false);
            
            Time.timeScale = 1;
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("StartScreen");
        }
    }
}

