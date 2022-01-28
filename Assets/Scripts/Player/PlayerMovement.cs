using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector2 movingVector, keyboardInputVector;
    [SerializeField]
    private float speed;
    private bool pressedActionButton, noInput, keyboardWasUsed;

    void Start()
    {
        pressedActionButton = false;
        keyboardWasUsed = false;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var gamepad = Gamepad.current;
        var keyboard = Keyboard.current;

        checkGamepadInput(gamepad);
        checkKeyBoardInput(keyboard);
    }

    private void checkGamepadInput (Gamepad gamepad)
    {
        if (gamepad.buttonSouth.wasPressedThisFrame) pressedActionButton = true;

        movingVector = gamepad.leftStick.ReadValue() * speed;
    }

    private void checkKeyBoardInput (Keyboard keyboard)
    {
        keyboardInputVector = Vector2.zero;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            keyboardInputVector.x -= 1;
            keyboardWasUsed = true;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            keyboardInputVector.x += 1;
            keyboardWasUsed = true;
        }

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            keyboardInputVector.y -= 1;
            keyboardWasUsed = true;
        }

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            keyboardInputVector.y += 1;
            keyboardWasUsed = true;
        }

        keyboardInputVector *= speed;

        if (keyboardWasUsed) movingVector = keyboardInputVector;
        keyboardWasUsed = false;

        if (keyboard.spaceKey.wasPressedThisFrame) pressedActionButton = true;
    }

    private void FixedUpdate()
    {
        if (pressedActionButton)
        {
            Debug.Log("pressed action button");
            pressedActionButton = false;
        }

        rigidBody.velocity = movingVector;

        noInput = movingVector == Vector2.zero;

        if (noInput) rigidBody.velocity = Vector2.zero;
    }
}
