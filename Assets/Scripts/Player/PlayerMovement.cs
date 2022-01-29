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
    private bool noInput, keyboardWasUsed;
    private Animator animator;
    private SpriteRenderer playerRenderer;

    void Start()
    {
        keyboardWasUsed = false;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        playerRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (PlayerHiding.Instance.isHiding) return;

        var gamepad = Gamepad.current;
        var keyboard = Keyboard.current;

        if (gamepad != null) checkGamepadInput(gamepad);
        checkKeyBoardInput(keyboard);
    }

    private void checkGamepadInput(Gamepad gamepad)
    {
        movingVector = gamepad.leftStick.ReadValue() * speed;
    }

    private void checkKeyBoardInput(Keyboard keyboard)
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

        keyboardInputVector.Normalize();

        keyboardInputVector *= speed;

        if (keyboardWasUsed) movingVector = keyboardInputVector;
        keyboardWasUsed = false;
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = movingVector;

        noInput = movingVector == Vector2.zero;

        if (noInput || PlayerHiding.Instance.isHiding)
        {
            rigidBody.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        if (rigidBody.velocity.x < 0) playerRenderer.flipX = true;
        if (rigidBody.velocity.x > 0) playerRenderer.flipX = false;
    }
}
