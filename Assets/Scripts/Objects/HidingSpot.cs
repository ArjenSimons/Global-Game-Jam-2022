using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField]
    private float interactionRadius = 2f;
    private float distanceToPlayer;
    private CapsuleCollider2D hidingSpotCollider;
    private bool holdingPlayer;

    void Start()
    {
        PlayerHiding.Instance.onPressHideButton += OnHideButtonPressed;
        hidingSpotCollider = gameObject.GetComponent<CapsuleCollider2D>();
        holdingPlayer = false;
    }

    void Update()
    {

    }

    private void OnHideButtonPressed()
    {
        if (holdingPlayer)
        {
            holdingPlayer = false;
            hidingSpotCollider.enabled = true;
            PlayerHiding.Instance.LeaveHidingSpot();
        }

        distanceToPlayer = Vector2.Distance(PlayerHiding.Instance.gameObject.transform.position, transform.position);
        if (distanceToPlayer <= interactionRadius && !holdingPlayer)
        {
            holdingPlayer = true;
            hidingSpotCollider.enabled = false;
            PlayerHiding.Instance.HideBehindObject(transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
