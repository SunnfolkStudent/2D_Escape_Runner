using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Jump")]
    public bool jumpPressed, jumpReleased, jumpHeld, jumpKey;

    [Header("Crouch")] 
    public bool crouchPressed, crouchReleased;
    
    [Header("movement")]
    public Vector2 moveVector;

    [Header("Sprint")] 
    public bool sprintHeld, SprintReleased;
    public float stamina = 3;

    private void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");

        jumpKey = Input.GetAxis("Jump") != 0;
        
        jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
        jumpReleased = Keyboard.current.spaceKey.wasReleasedThisFrame;
        jumpHeld = Keyboard.current.spaceKey.isPressed;
        
        crouchPressed = Keyboard.current.leftCtrlKey.wasPressedThisFrame;
        crouchReleased = Keyboard.current.leftCtrlKey.wasReleasedThisFrame;
        
        sprintHeld = Keyboard.current.leftShiftKey.isPressed;
        SprintReleased = Keyboard.current.leftShiftKey.wasReleasedThisFrame;
    }
}