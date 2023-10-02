using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 moveVector;
    public bool jumpPressed;
    public bool jumpReleased;
    public bool crouchPressed;
    public bool crouchReleased;
    public bool sprintPressed;
    public bool sprintReleased;
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
        moveVector = _controls.Player.Move.ReadValue<Vector2>();
        jumpPressed = _controls.Player.Jump.triggered;
        jumpReleased = _controls.Player.Jump.WasReleasedThisFrame();
        crouchPressed = _controls.Player.Crouch.triggered;
        crouchReleased = _controls.Player.Crouch.WasReleasedThisFrame();
        sprintPressed = _controls.Player.Sprint.triggered;
        sprintReleased = _controls.Player.Sprint.WasReleasedThisFrame();
    }
}