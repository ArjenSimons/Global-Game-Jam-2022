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

    private Vector3 patrollDestination;
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

        dayNightManager.OnDayStart += OnDayStart;
        dayNightManager.OnNightStart += OnNightStart;
        dayNightManager.OnNightEnd += OnNightEnd;
        
    }

    private void OnDestroy()
    {
        dayNightManager.OnDayStart -= OnDayStart;
        dayNightManager.OnNightStart -= OnNightStart;
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
        else
        {
            Patroll();
        }
    }

    private void OnNightStart()
    {
        //TODO: Set Patroll target position

        patrollDestination = GetRandomPatrollPoint();

        agent.SetDestination(patrollDestination);
    }

    private void OnNightEnd()
    {
        agent.ResetPath();
        //TODO: Set Flee position
        //TODO: Go to hiding spot
        //TODO: Determine whether to hide or not

    }

    private void OnDayStart()
    {
    }

    private void Patroll()
    {
        if (Vector3.SqrMagnitude(transform.position - patrollDestination) < .5f)
        {
            patrollDestination = GetRandomPatrollPoint();
            agent.SetDestination(patrollDestination);
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

    private Vector3 GetRandomPatrollPoint()
    {
        int randomIndex = Random.Range(0, PatrollPoint.Instances.Count);
        return PatrollPoint.Instances[randomIndex].transform.position;
    }

    private Vector3 GetRandomVector2D()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        Vector2 dest = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return new Vector3(dest.x, dest.y, 0).normalized;
    }
}
