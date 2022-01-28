using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHiding : MonoBehaviour
{
    private bool pressedHideButton;

    void Start()
    {
        pressedHideButton = false;
    }

    void Update()
    {
        var gamepad = Gamepad.current;
        var keyboard = Keyboard.current;

        if (gamepad.buttonSouth.wasPressedThisFrame) pressedHideButton = true;

        if (keyboard.spaceKey.wasPressedThisFrame) pressedHideButton = true;
    }

    private void FixedUpdate()
    {
        if (pressedHideButton)
        {
            Debug.Log("pressed action button");
            pressedHideButton = false;
        }
    }
}
