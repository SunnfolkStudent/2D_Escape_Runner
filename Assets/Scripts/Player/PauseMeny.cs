using UnityEngine;

namespace Player
{
    public class PauseMeny : MonoBehaviour
    {
        private InputManager _input;
        
        public bool isPaused;

        private void Start()
        {
            _input = GetComponent<InputManager>();
            
        }

        void Update()
        {
            if (_input.pause)
            {
                if (!isPaused)
                {
                    Time.timeScale = 0;
                    Cursor.visible = true;
                    isPaused = true;
                }

                else
                {
                    Time.timeScale = 1;
                    Cursor.visible = false;
                    isPaused = false;
                }
            }
        }
    }
}

