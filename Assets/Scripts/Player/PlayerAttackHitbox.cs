using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{

    private GameObject villagerTarget, hidingSpotTarget;
    
    void Start()
    {
        PlayerAttack.Instance.onPressAttackButtonEvent += OnAttack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Villager")) villagerTarget = collision.gameObject;
        if (collision.CompareTag("HidingSpot")) hidingSpotTarget = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        villagerTarget = null;
        hidingSpotTarget = null;
    }

    private void OnAttack ()
    {
        if (villagerTarget != null) villagerTarget.GetComponent<Enemy>().KillEnemy();
        if (hidingSpotTarget != null) hidingSpotTarget.GetComponent<HidingSpotDestroy>().DestroyThisHidingSpot();
    }
}
