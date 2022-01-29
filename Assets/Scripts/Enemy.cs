using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fleeDistance;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;


    private DayNightManager dayNightManager;
    private float fleeDistanceSquared;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        dayNightManager = DayNightManager.Instance;

        fleeDistanceSquared = fleeDistance * fleeDistance;
    }

    private void Update()
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
            Flee();
        }
    }

    private void HandleNightBehaviour()
    {
        agent.SetDestination(player.position);
    }

    private void Flee()
    {
        Vector3 runTo = transform.position + ((transform.position - player.position) * 3);

        agent.SetDestination(runTo);
    }
}
