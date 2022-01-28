using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector2 movingVector;
    [SerializeField]
    private float speed;
    private bool pressedActionButton, noInput;

    void Start()
    {
        pressedActionButton = false;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var gamepad = Gamepad.current;

        if (gamepad == null) return;

        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            pressedActionButton = true;
        }

        movingVector = gamepad.leftStick.ReadValue() * speed;
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

        if (noInput)
        {
            rigidBody.velocity = Vector2.zero;
        }
    }
}
