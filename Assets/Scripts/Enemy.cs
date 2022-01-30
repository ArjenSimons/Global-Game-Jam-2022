using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float hideSpeed;
    [SerializeField] private float fleeDistance = 4;
    [SerializeField] private float chaseDistance = 4;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private DayNightManager dayNightManager;
    private HidingSpotManager hidingSpotManager;
    private Transform player;

    private Vector3 patrollDestination;
    private float fleeDistanceSquared;
    private float chaseDistanceSquared;
    private bool isChasing;

    private HidingSpot currentHidingSpot;
    private bool isHiding;

    private int updateDelay = 5;
    private float timer;
    private float patrollTimer;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        dayNightManager = DayNightManager.Instance;
        hidingSpotManager = HidingSpotManager.Instance;
        player = Player.Instance.transform;

        agent.speed = speed;

        fleeDistanceSquared = fleeDistance * fleeDistance;
        chaseDistanceSquared = chaseDistance * chaseDistance;

        dayNightManager.OnDayStart += OnDayStart;
        dayNightManager.OnDayEnd += OnDayEnd;
        dayNightManager.OnNightStart += OnNightStart;
        dayNightManager.OnNightEnd += OnNightEnd;
        dayNightManager.OnTransitionToNightStart += OnTransitionToNightStart;
    }

    private void OnDestroy()
    {
        dayNightManager.OnDayStart -= OnDayStart;
        dayNightManager.OnDayEnd -= OnDayEnd;
        dayNightManager.OnNightStart -= OnNightStart;
        dayNightManager.OnNightEnd -= OnNightEnd;
        dayNightManager.OnTransitionToNightStart -= OnTransitionToNightStart;
    }

    private void FixedUpdate()
    {
        if (dayNightManager.CurrentDayState == DayState.DAY)
        {
            HandleDayBehaviour();
        }
        else if (currentHidingSpot != null && !isHiding)
        {
            if ((transform.position - currentHidingSpot.transform.position).sqrMagnitude < 1.5f)
            {
                Hide();
            }
        }
        else if (dayNightManager.CurrentDayState == DayState.NIGHT)
        {
            HandleNightBehaviour();
        }

        

        //if (!isHiding)
        //{
        //    if (agent.velocity.magnitude < float.Epsilon && agent.hasPath)
        //    {
        //        Debug.Log("yeet");
        //        agent.ResetPath();
        //        agent.SetDestination(GetRandomPatrollPoint());
        //        currentHidingSpot = null;
        //        isChasing = false;
        //    }
        //}
    }

    private void HandleDayBehaviour()
    {
        if (!PlayerHiding.Instance.isHiding && Vector3.SqrMagnitude(transform.position - player.position) < fleeDistanceSquared)
        {
            Chase();
        }
        else
        {
            Patroll();
        }
    }

    private void HandleNightBehaviour()
    {
        //TODO: Patroll when not close to player

        if (currentHidingSpot != null) return;

        if (!PlayerHiding.Instance.isHiding && Vector3.SqrMagnitude(transform.position - player.position) < chaseDistanceSquared)
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
        //currentHidingSpot = null;
        //patrollDestination = GetRandomPatrollPoint();
        //agent.ResetPath();
        //agent.SetDestination(patrollDestination);
        if (isHiding) return;

        if (currentHidingSpot != null)
        {
            hidingSpotManager.StopHidingSpotUsage(currentHidingSpot);
            currentHidingSpot = null;
        }

        agent.speed = speed;
        agent.acceleration = 20;

        agent.ResetPath();
        agent.SetDestination(GetRandomPatrollPoint());
    }

    private void OnDayEnd()
    {


    }

    private void OnTransitionToNightStart()
    {
        if (Random.Range(.0f, 1.0f) <= 0.8f)
        {

            currentHidingSpot = hidingSpotManager.ClaimRandomHidingSpot();

            agent.ResetPath();
            if (currentHidingSpot != null)
            {
                agent.SetDestination(currentHidingSpot.transform.position);
                agent.speed = hideSpeed;
                agent.acceleration = 100;
            }
            else
            {
                agent.SetDestination(GetRandomPatrollPoint());
            }
        }
        else
        {
            agent.SetDestination(GetRandomPatrollPoint());
        }
        //TODO: Set Flee position
        //TODO: Determine whether to hide or not
    }

    private void OnDayStart()
    {
        if (currentHidingSpot != null)
        {
            hidingSpotManager.StopHidingSpotUsage(currentHidingSpot);
            currentHidingSpot = null;
            isHiding = false;
        }
        patrollDestination = GetRandomPatrollPoint();
        agent.ResetPath();
        agent.SetDestination(patrollDestination);
    }

    private void OnNightEnd()
    {
        agent.speed = speed;
        agent.acceleration = 20;

        LeaveHidingSpot();

        //TODO: Leave hiding spot
        //TODO: 
        agent.ResetPath();


    }

    private void Patroll()
    {
        if (isChasing)
        {
            agent.SetDestination(GetRandomPatrollPoint());
            isChasing = false;
            patrollTimer = 0;
        }

        patrollTimer += Time.deltaTime;

        if (Vector3.SqrMagnitude(transform.position - patrollDestination) < 1.5f)
        {
            patrollDestination = GetRandomPatrollPoint();
            agent.SetDestination(patrollDestination);
            patrollTimer = 0.0f;
        }
    }

    private void Chase()
    {
        isChasing = true;
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

    private void Hide()
    {
        agent.ResetPath();
        transform.position = currentHidingSpot.transform.position + (Vector3.up * 0.01f);
        spriteRenderer.enabled = false;
        isHiding = true;
    }

    private void LeaveHidingSpot()
    {
        if (!isHiding) return;
        isHiding = false;
        //TODO: play animation
        transform.Translate(Vector2.right * 2);
        spriteRenderer.enabled = true;
        hidingSpotManager.StopHidingSpotUsage(currentHidingSpot);
        currentHidingSpot = null;

    }

    private Vector3 GetRandomPatrollPoint()
    {
        //List<PatrollPoint> pointsInRange = new List<PatrollPoint>();
        //foreach (PatrollPoint p in PatrollPoint.AvailableParollPoints)
        //{
        //    if ((p.transform.position - transform.position).sqrMagnitude < 2 * 2)
        //    {
        //        pointsInRange.Add(p);
        //    }
        //}

        //if (pointsInRange.Count > 0)
        //{
        //    int randomIndex = Random.Range(0, pointsInRange.Count);
        //    return pointsInRange[randomIndex].transform.position;
        //}
        //else
        //{

        if (PatrollPoint.AvailableParollPoints.Count < 5)
        {
            int randomIndex = Random.Range(0, PatrollPoint.AvailableParollPoints.Count);
            return PatrollPoint.AvailableParollPoints[randomIndex].transform.position;
        }
        else
        {
            int randomIndex = Random.Range(0, PatrollPoint.Instances.Count);
            return PatrollPoint.Instances[randomIndex].transform.position;
        }
        //}
    }
}
