using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    private void Update()
    {
        agent.SetDestination(player.position);

    }
}
