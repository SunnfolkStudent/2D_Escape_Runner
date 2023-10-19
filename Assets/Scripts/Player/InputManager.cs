using UnityEngine;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        public float moveVector;
        public bool jumpPressed, jumpReleased;
        public bool crouchPressed, crouchReleased;
        public bool sprintPressed, sprintReleased;
        private Controls _controls;

        private void Awake()
        {
            _controls = new Controls();
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void Update()
        {
            moveVector = _controls.Player.Move.ReadValue<float>();
            jumpPressed = _controls.Player.Jump.triggered;
            jumpReleased = _controls.Player.Jump.WasReleasedThisFrame();
            crouchPressed = _controls.Player.Crouch.triggered;
            crouchReleased = _controls.Player.Crouch.WasReleasedThisFrame();
            sprintPressed = _controls.Player.Sprint.triggered;
            sprintReleased = _controls.Player.Sprint.WasReleasedThisFrame();
        }
    }
}