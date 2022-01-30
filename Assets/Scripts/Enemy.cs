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

    private HidingSpot currentHidingSpot;
    private bool isHiding;

    private int updateDelay = 5;
    private float timer;

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
        else if (dayNightManager.CurrentDayState == DayState.NIGHT)
        {
            HandleNightBehaviour();
        }

        if (currentHidingSpot != null && !isHiding)
        {
            //Debug.Log("running to hide spot");
            //Debug.Log((transform.position - currentHidingSpot.transform.position).sqrMagnitude);
            //Debug.Log(currentHidingSpot.transform.position);
            if ((transform.position - currentHidingSpot.transform.position).sqrMagnitude < 1.5f)
            {
                Hide();   
            }
        }
    }

    private void HandleDayBehaviour()
    {
        if (Vector3.SqrMagnitude(transform.position - player.position) < fleeDistanceSquared)
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

        if (currentHidingSpot == null) return;

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

    }

    private void OnDayEnd()
    {


    }

    private void OnTransitionToNightStart()
    {
        currentHidingSpot = hidingSpotManager.ClaimRandomHidingSpot();

        if (currentHidingSpot != null)
        {
            agent.SetDestination(currentHidingSpot.transform.position);
            agent.speed = hideSpeed;
            agent.acceleration = 100;
        }

        //TODO: Set Flee position
        //TODO: Determine whether to hide or not
    }

    private void OnDayStart()
    {
        patrollDestination = GetRandomPatrollPoint();

        agent.SetDestination(patrollDestination);
    }

    private void OnNightEnd()
    {
        agent.speed = speed;
        agent.acceleration = 100;

        LeaveHidingSpot();

        //TODO: Leave hiding spot
        //TODO: 
        agent.ResetPath();
        

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

    private void Hide()
    {
        agent.ResetPath();
        currentHidingSpot.enemy = this;
        transform.position = currentHidingSpot.transform.position + (Vector3.up * 0.01f);
        spriteRenderer.enabled = false;
        isHiding = true;
    }

    public void LeaveHidingSpot()
    {
        if (!isHiding) return;
        currentHidingSpot.enemy = null;
        isHiding = false;
        //TODO: play animation
        transform.Translate(Vector2.right * 2);
        currentHidingSpot = null;
        spriteRenderer.enabled = true;
    }

    private Vector3 GetRandomPatrollPoint()
    {
        int randomIndex = Random.Range(0, PatrollPoint.AvailableParollPoints.Count);
        return PatrollPoint.AvailableParollPoints[randomIndex].transform.position;
    }
}
