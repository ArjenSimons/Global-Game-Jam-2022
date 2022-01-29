using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector2 movingVector, keyboardInputVector;
    [SerializeField]
    private float speed;
    private bool noInput, keyboardWasUsed;
    private Animator monsterNightAnimator, monsterDayAnimator;
    private SpriteRenderer monsterNightRenderer, monsterDayRenderer;
    [SerializeField]
    private GameObject monsterNight, monsterDay;

    void Start()
    {
        keyboardWasUsed = false;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        monsterNightAnimator = monsterNight.GetComponent<Animator>();
        monsterDayAnimator = monsterDay.GetComponent<Animator>();
        monsterNightRenderer = monsterNight.GetComponent<SpriteRenderer>();
        monsterDayRenderer = monsterDay.GetComponent<SpriteRenderer>();
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

        bool isNightMonster = PlayerDayNightSwitch.Instance.isNightMonster;

        if (noInput || PlayerHiding.Instance.isHiding)
        {
            rigidBody.velocity = Vector2.zero;
            if (isNightMonster) monsterNightAnimator.SetBool("isWalking", false);
            if (!isNightMonster) monsterDayAnimator.SetBool("isWalking", false);
        }
        else
        {
            if (isNightMonster) monsterNightAnimator.SetBool("isWalking", true);
            if (!isNightMonster) monsterDayAnimator.SetBool("isWalking", true);
        }



        if (isNightMonster && rigidBody.velocity.x < 0) monsterNightRenderer.flipX = true;
        if (isNightMonster && rigidBody.velocity.x > 0) monsterNightRenderer.flipX = false;

        if (!isNightMonster && rigidBody.velocity.x < 0) monsterDayRenderer.flipX = true;
        if (!isNightMonster && rigidBody.velocity.x > 0) monsterDayRenderer.flipX = false;
    }
}
