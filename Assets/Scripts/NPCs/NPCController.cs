using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    //[SerializeField] private List<Transform> pathingWaypoints;
    [SerializeField] private Animator _animator;
    //[SerializeField] private NavMeshAgent _agent;

    [Header("Idle Animation Info")]
    [SerializeField] private float minTimeToWait = 5f;
    [SerializeField] private float maxTimeToWait = 20f;
    [SerializeField] private bool isDefaultIdling = true;

    //private int nextWaypointIndex = 0;
    //private bool npcMoving = false;
    private Coroutine idleCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        isDefaultIdling = true;
        //nextWaypointIndex = 0;
        _animator = GetComponentInChildren<Animator>();
        //_agent = GetComponent<NavMeshAgent>();

        // play random idle animation
        idleCoroutine = StartCoroutine(PlayRandomIdleAnim());
    }

    private void Update()
    {
        // Restart the idle animation coroutine after reaching waypoint
        if (idleCoroutine == null)
        {
            idleCoroutine = StartCoroutine(PlayRandomIdleAnim());
        }
        /*
        if (npcMoving)
        {
            // Check if the agent has reached its destination
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                // Set animation back to idle
                _animator.SetTrigger("Idle");

                FaceWaypoint();
                npcMoving = false;

                
            }
        }
        */
    }

    private IEnumerator PlayRandomIdleAnim()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeToWait, maxTimeToWait));
            ChangeIdleAnim(isDefaultIdling);
            isDefaultIdling = !isDefaultIdling;
        }
    }

    private void ChangeIdleAnim(bool isDefaultIdle)
    {
        if (isDefaultIdle)
        {
            _animator.SetTrigger("IdleOther");
        } else
        {
            _animator.SetTrigger("Idle");
        }
    }

    /*
    public bool IsMoving()
    {
        return npcMoving;
    }

    public void GoToNextWaypoint()
    {
        npcMoving = true;

        // Stop the idle coroutine when starting to move
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }

        Transform nextWaypoint = pathingWaypoints[nextWaypointIndex % pathingWaypoints.Count];
        _agent.SetDestination(nextWaypoint.position);
        _animator.SetTrigger("Walk");

        // update waypoint
        nextWaypointIndex++;
    }

    private void FaceWaypoint()
    {
        // Get the forward direction of the waypoint
        Transform currentWaypoint = pathingWaypoints[(nextWaypointIndex - 1) % pathingWaypoints.Count];
        Vector3 targetDirection = currentWaypoint.forward;

        // Rotate the NPC to face the target direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = targetRotation;
    }
    */

}
