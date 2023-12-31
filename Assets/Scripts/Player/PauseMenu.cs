using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private InputManager input;
        private GameObject _player;
        
        [Header("PauseMenu")]
        [SerializeField] private  bool isPaused;
        [SerializeField] private GameObject paused;
        [SerializeField] private  GameObject resume;
        [SerializeField] private  GameObject mainMenu;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            Unpause();
        }

        private void Update()
        {
            if (isPaused && input.crouchPressed)
            { 
                MainMenu();
            }

            if (isPaused && input.jumpPressed)
            {
                Unpause();
            }
            
            if (!input.pause) return;
            
            if (!isPaused)
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
            isPaused = true;
            Cursor.visible = true;
            
            paused.SetActive(true);
            resume.SetActive(true);
            mainMenu.SetActive(true);
            
            Time.timeScale = 0;

            _player.GetComponent<PlayerController>().enabled = false;
        }

        public void Unpause()
        {
            isPaused = false;
            Cursor.visible = false;
            
            paused.SetActive(false);
            resume.SetActive(false);
            mainMenu.SetActive(false);
            
            Time.timeScale = 1;
            
            _player.GetComponent<PlayerController>().enabled = true;
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("StartScreen");
        }
    }
}