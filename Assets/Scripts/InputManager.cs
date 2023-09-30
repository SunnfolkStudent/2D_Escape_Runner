using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Jump")]
    [HideInInspector] public bool jumpPressed, jumpReleased;

    [Header("Crouch")] 
    [HideInInspector] public bool crouchPressed, crouchReleased;
    
    [Header("movement")]
    [HideInInspector] public Vector2 moveVector;

    [Header("Sprint")] 
    [HideInInspector] public bool sprintPressed, sprintReleased, sprintHeld;

    private Keyboard _keyboard;
    private Gamepad _gamepad;
    [SerializeField] private bool usingGamepad, usingKeyboard;


    private void Start()
    {
        _keyboard = Keyboard.current;
        _gamepad = Gamepad.current;
    }

    private void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");

        if (_gamepad.wasUpdatedThisFrame)
        {
            usingGamepad = true;
            usingKeyboard = false;
        }
        else if (_keyboard.wasUpdatedThisFrame)
        {
            usingGamepad = false;
            usingKeyboard = true;
        }

        if (usingGamepad && _gamepad != null)
        {
           GamepadControls();
        }
        else if (usingKeyboard && _keyboard != null)
        {
           KeyboardControls();  
        }
    }

    private void KeyboardControls()
    {
              jumpPressed = _keyboard.spaceKey.wasPressedThisFrame;
              jumpReleased = _keyboard.spaceKey.wasReleasedThisFrame;
              
              crouchPressed = _keyboard.leftCtrlKey.wasPressedThisFrame;
              crouchReleased = _keyboard.leftCtrlKey.wasReleasedThisFrame;
              
              sprintPressed = _keyboard.leftShiftKey.wasPressedThisFrame;
              sprintReleased = _keyboard.leftShiftKey.wasReleasedThisFrame;
              sprintHeld = _keyboard.leftShiftKey.isPressed;  
    }
    
    private void GamepadControls()
    {
        jumpPressed = _gamepad.buttonSouth.wasPressedThisFrame;
        jumpReleased = _gamepad.buttonSouth.wasReleasedThisFrame;
              
        crouchPressed = _gamepad.buttonEast.wasPressedThisFrame;
        crouchReleased = _gamepad.buttonEast.wasReleasedThisFrame;
              
        sprintPressed = _gamepad.leftStickButton.wasPressedThisFrame;
        sprintReleased =_gamepad.leftStickButton.wasReleasedThisFrame;
        sprintHeld = _gamepad.leftStickButton.isPressed; 
    }
}