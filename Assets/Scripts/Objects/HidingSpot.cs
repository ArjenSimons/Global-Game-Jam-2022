using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField]
    private float interactionRadius = 2f;
    private float distanceToPlayer;
    private BoxCollider2D hidingSpotCollider;
    private bool holdingPlayer;
    public Enemy enemy;

    private void Awake()
    {
        HidingSpotManager.Instance.AddHidingSpot(this);
    }

    void Start()
    {
        PlayerHiding.Instance.onPressHideButton += OnHideButtonPressed;
        hidingSpotCollider = gameObject.GetComponent<BoxCollider2D>();
        holdingPlayer = false;
        DayNightManager.Instance.OnNightStart += MakePlayerLeave;
    }

    private void OnHideButtonPressed()
    {
        if (holdingPlayer)
        {
            MakePlayerLeave();
        }

        distanceToPlayer = Vector2.Distance(PlayerHiding.Instance.gameObject.transform.position, transform.position);
        if (distanceToPlayer <= interactionRadius && !holdingPlayer)
        {
            holdingPlayer = true;
            hidingSpotCollider.enabled = false;
            PlayerHiding.Instance.HideBehindObject(transform);
        }
    }

    private void MakePlayerLeave ()
    {
        if (!holdingPlayer) return;
        holdingPlayer = false;
        hidingSpotCollider.enabled = true;
        PlayerHiding.Instance.LeaveHidingSpot();
    }

    private void OnDestroy()
    {
        if (enemy != null) enemy.LeaveHidingSpot();
        HidingSpotManager.Instance.RemoveHidingSpot(this);
        PlayerHiding.Instance.onPressHideButton -= OnHideButtonPressed;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
#endif
}
