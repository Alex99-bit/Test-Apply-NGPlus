using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCPedestrian : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float wanderRadius = 20f;
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;
    public float playerAvoidanceRadius = 5f;

    [Header("Behavior")]
    public bool shouldAvoidPlayer = true;
    public bool useRandomAnimations = true;

    private NavMeshAgent agent;
    private Transform player;
    private NPCState currentState;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float waitTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        startPosition = transform.position;
        currentState = NPCState.Walking;
        agent.speed = walkSpeed;
        SetNewDestination();
    }

    void Update()
    {
        switch (currentState)
        {
            case NPCState.Walking:
                UpdateWalkingState();
                break;
            case NPCState.Waiting:
                UpdateWaitingState();
                break;
            case NPCState.Fleeing:
                UpdateFleeingState();
                break;
        }

        if (shouldAvoidPlayer)
            CheckPlayerProximity();
    }

    private void UpdateWalkingState()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            currentState = NPCState.Waiting;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    private void UpdateWaitingState()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0)
        {
            currentState = NPCState.Walking;
            SetNewDestination();
        }
    }

    private void UpdateFleeingState()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (distanceToPlayer > playerAvoidanceRadius * 2)
            {
                currentState = NPCState.Walking;
                agent.speed = walkSpeed;
                SetNewDestination();
            }
        }
    }

    private void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += startPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas);
        targetPosition = hit.position;
        agent.SetDestination(targetPosition);
    }

    private void CheckPlayerProximity()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (distanceToPlayer < playerAvoidanceRadius && currentState != NPCState.Fleeing)
            {
                FleeFromPlayer();
            }
        }
    }

    private void FleeFromPlayer()
    {
        if (player != null)
        {
            currentState = NPCState.Fleeing;
            agent.speed = runSpeed;
            
            Vector3 directionFromPlayer = transform.position - player.position;
            Vector3 fleePosition = transform.position + directionFromPlayer.normalized * playerAvoidanceRadius * 2;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleePosition, out hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        
        if (shouldAvoidPlayer)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, playerAvoidanceRadius);
        }
    }
}

public enum NPCState
{
    Walking,
    Waiting,
    Fleeing
}