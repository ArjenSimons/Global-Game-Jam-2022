using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private bool pressedAttackButton, ableToAttack;
    public event Action onPressAttackButtonEvent;
    [SerializeField]
    private GameObject monsterNight;
    private Animator monsterNightAnimator;

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

        DayNightManager.Instance.OnDayStart += OnDay;
        DayNightManager.Instance.OnNightStart += OnNight;
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
}
