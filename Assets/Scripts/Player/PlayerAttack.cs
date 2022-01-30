using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    private bool pressedAttackButton, ableToAttack;
    public event Action onPressAttackButtonEvent;
    [SerializeField]
    private GameObject monsterNight;
    private Animator monsterNightAnimator;
    [SerializeField] private TextMeshProUGUI killtext;
    [SerializeField] int amountOfEnemies;
    public int amountOfEnemiesLeft;

    public static PlayerAttack Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerAttack>();
                if (instance == null)
                {
                    GameObject go = new GameObject("PlayerAttack");
                    instance = go.AddComponent<PlayerAttack>();
                }
            }
            return instance;
        }
    }

    private static PlayerAttack instance;

    void Start()
    {
        pressedAttackButton = false;
        ableToAttack = true;
        monsterNightAnimator = monsterNight.GetComponent<Animator>();

        amountOfEnemiesLeft = amountOfEnemies;
        UpdateKillText();

        DayNightManager.Instance.OnDayStart += OnDay;
        DayNightManager.Instance.OnNightStart += OnNight;
    }

    public void UpdateKillText ()
    {
        killtext.text = amountOfEnemiesLeft + "/" + amountOfEnemies;
        CheckGameWin();
    }

    private void OnDay()
    {
        ableToAttack = false;
    }

    private void OnNight()
    {
        ableToAttack = true;
    }

    void Update()
    {
        var gamepad = Gamepad.current;
        var keyboard = Keyboard.current;

        if (gamepad != null && gamepad.buttonSouth.wasPressedThisFrame) pressedAttackButton = true;

        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame) pressedAttackButton = true;
    }

    private void FixedUpdate()
    {
        if (pressedAttackButton && ableToAttack)
        {
            Debug.Log("pressed action button");
            onPressAttackButtonEvent?.Invoke();
            Attack();
            pressedAttackButton = false;
        }
    }

    public void Attack()
    {
        monsterNightAnimator.SetTrigger("doAttack");
    }

    public void CheckGameWin ()
    {
        if (amountOfEnemiesLeft <= 0)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
