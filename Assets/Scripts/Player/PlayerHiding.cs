using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHiding : MonoBehaviour
{
    private bool pressedHideButton, ableToHide;
    public bool isHiding;
    public event Action onPressHideButton;

    public static PlayerHiding Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerHiding>();
                if (instance == null)
                {
                    GameObject go = new GameObject("PlayerHiding");
                    instance = go.AddComponent<PlayerHiding>();
                }
            }
            return instance;
        }
    }

    private static PlayerHiding instance;

    void Start()
    {
        pressedHideButton = false;
        ableToHide = true;
        isHiding = false;

        DayNightManager.Instance.OnDayStart += OnDay;
        DayNightManager.Instance.OnNightStart += OnNight;
    }

    private void OnDay ()
    {
        ableToHide = true;
    }

    private void OnNight()
    {
        ableToHide = false;
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
        if (pressedHideButton && ableToHide)
        {
            Debug.Log("pressed action button");
            onPressHideButton?.Invoke();
            pressedHideButton = false;
        }
    }

    public void HideBehindObject (Transform hidingSpot)
    {
        isHiding = true;
        transform.position = hidingSpot.position + (Vector3.up * 0.01f);
    }

    public void LeaveHidingSpot ()
    {
        if (!isHiding) return;
        isHiding = false;
        // play animation
        transform.Translate(Vector2.right * 2);
    }
}
