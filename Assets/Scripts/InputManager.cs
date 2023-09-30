using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Jump")]
    [HideInInspector] public bool jumpPressed, jumpReleased, jumpKey, jumpHeld;

    [Header("Crouch")] 
    [HideInInspector] public bool crouchPressed, crouchReleased;
    
    [Header("movement")]
    [HideInInspector] public Vector2 moveVector;

    [Header("Sprint")] 
    [HideInInspector] public bool sprintPressed, sprintReleased, sprintHeld;

    private void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");

        jumpKey = Input.GetAxis("Jump") != 0;

        jumpPressed = (Keyboard.current.spaceKey.wasPressedThisFrame) || (Gamepad.current.aButton.wasPressedThisFrame);
        jumpReleased = Keyboard.current.spaceKey.wasReleasedThisFrame || (Gamepad.current.aButton.wasReleasedThisFrame);
        jumpHeld = Keyboard.current.spaceKey.isPressed || (Gamepad.current.aButton.isPressed);
        
        crouchPressed = Keyboard.current.leftCtrlKey.wasPressedThisFrame || (Gamepad.current.bButton.wasPressedThisFrame);
        crouchReleased = Keyboard.current.leftCtrlKey.wasReleasedThisFrame || (Gamepad.current.bButton.wasReleasedThisFrame);
        
        sprintPressed = Keyboard.current.leftShiftKey.wasPressedThisFrame || (Gamepad.current.leftStickButton.wasPressedThisFrame);
        sprintReleased = Keyboard.current.leftShiftKey.wasReleasedThisFrame || (Gamepad.current.leftStickButton.wasReleasedThisFrame);
        sprintHeld = Keyboard.current.leftShiftKey.isPressed || (Gamepad.current.leftStickButton.isPressed);
    }
}