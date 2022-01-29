using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fleeDistance = 4;
    [SerializeField] private float chaseDistance = 4;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;


    private DayNightManager dayNightManager;
    private float fleeDistanceSquared;
    private float chaseDistanceSquared;
    private int updateDelay = 5;
    private float timer;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        dayNightManager = DayNightManager.Instance;

        fleeDistanceSquared = fleeDistance * fleeDistance;
        chaseDistanceSquared = chaseDistance * chaseDistance;
    }

    private void FixedUpdate()
    {
        if (dayNightManager.CurrentDayState == DayState.DAY)
        {
            HandleDayBehaviour();
        }
        else if (dayNightManager.CurrentDayState == DayState.NIGHT)
        {
            HandleNightBehaviour();
        }

    }

    private void HandleDayBehaviour()
    {
        if (Vector3.SqrMagnitude(transform.position - player.position) < fleeDistanceSquared)
        {
            Chase();
        }
    }

    private void HandleNightBehaviour()
    {
        //TODO: Patroll when not close to player

        if (Vector3.SqrMagnitude(transform.position - player.position) < chaseDistanceSquared)
        {
            Flee();
        }
    }

    private void Chase()
    {
        timer++;

        if (timer >= updateDelay)
        {
            agent.SetDestination(player.position);
        }
    }

    private void Flee()
    {
        timer++;
        Vector3 runTo = transform.position + ((transform.position - player.position).normalized * 3);

        if (timer >= updateDelay)
        {
            agent.SetDestination(runTo);
            timer = 0;
        }
    }
}
