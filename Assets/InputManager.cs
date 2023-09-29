using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public bool jumpPressed;
    public bool jumpReleased;
    public bool jumpHeld;
    public bool jumpKey;

    public Vector2 moveVector;

    private void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");

        jumpKey = Input.GetAxis("Jump") != 0;
        
        jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
        jumpReleased = Keyboard.current.spaceKey.wasReleasedThisFrame;
        jumpHeld = Keyboard.current.spaceKey.isPressed;
    }
}