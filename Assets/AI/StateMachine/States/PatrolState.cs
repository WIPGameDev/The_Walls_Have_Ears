using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "PatrolState", menuName = "AI FSM State/States/Patrol", order = 5)]
public class PatrolState : AbstractFMSState
{
    [SerializeField]
    sbyte floor = 0;
    
    byte localIndex;
    
    byte globalIndex;

    byte previousIndex = 255;

    [SerializeField]
    LayerMask detectLayer;

    [SerializeField]
    float MaxMovementTime = 30f;

    [SerializeField]
    float waitTime = 1.5f;

    float timeMoving = 0f;

    public override void OnEnable()
    {
        base.OnEnable();
        StateType = FSMStateType.PATROL;
    }

    public override bool EnterState()
    {
        EnteredState = base.EnterState();

        if (EnteredState)
        {

            globalIndex = 200;

            float storedDist = float.MaxValue;

            for (int i = 0; i < hiveMind.patrolPoints[floor].Count; i++)
            {
                float dist = Vector3.Distance(navMeshAgent.transform.position, hiveMind.patrolPoints[floor][i].transform.position);

                if (dist < storedDist)
                {
                    storedDist = dist;
                    globalIndex = (byte)i;
                }
            }

            if (globalIndex == 255)
            {
                Debug.LogError("Failed to enter patrol state: index = 255");
                return false;
            }

            NavMeshHit hit;

            NavMesh.SamplePosition(hiveMind.patrolPoints[floor][globalIndex].transform.position, 
                out hit, 10, NavMesh.AllAreas);

            navMeshAgent.SetDestination(hit.position);

            Debug.Log("Entered patrol state");
        }

        return EnteredState;
    }

    public override bool ReEnterState(AbstractFMSState state)
    {
        localIndex = 255;

        NavMeshHit hit;

        if (hiveMind.patrolPoints[floor][globalIndex].linkedPoints.Count == 1)
        {
            localIndex = 0;
        }
        else
        {
            if (hiveMind.patrolPoints[floor][globalIndex].linkedPoints.Count == 2)
            {
                localIndex = 0;

                if (hiveMind.patrolPoints[floor][globalIndex] != hiveMind.patrolPoints[floor][0])
                    localIndex = 1;
            }
            else
            {
                System.Random rng = new System.Random();

                if (previousIndex != 255)
                {
                    byte foundIndex = 255;

                    foundIndex = (byte)hiveMind.patrolPoints[floor][globalIndex].linkedPoints.IndexOf(hiveMind.patrolPoints[floor][previousIndex].gameObject);
                    do
                    {
                        localIndex = (byte)rng.Next(hiveMind.patrolPoints[floor][globalIndex].linkedPoints.Count);
                    } while (localIndex == foundIndex);

                }
                else
                    localIndex = (byte)rng.Next(hiveMind.patrolPoints[floor][globalIndex].linkedPoints.Count);
            }
        }

        previousIndex = globalIndex;

        NavMesh.SamplePosition(hiveMind.patrolPoints[floor][globalIndex].linkedPoints[localIndex].transform.position,
               out hit, 10, NavMesh.AllAreas);
        navMeshAgent.SetDestination(hit.position);

        globalIndex = (byte)hiveMind.patrolPoints[floor].IndexOf(hiveMind.patrolPoints[floor][globalIndex].linkedPoints[localIndex].GetComponent<PatrolPoints>());

        timeMoving = 0;

        return true;
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
            Debug.Log("Updating patrol state");

            timeMoving += Time.deltaTime;

            if (timeMoving >= MaxMovementTime)
            {
                EnterState();
                timeMoving = 0;
            }

            if (Vector3.Distance(navMeshAgent.transform.position, navMeshAgent.destination) < 1f)
                fsm.EnterState(FSMStateType.IDLE, waitTime);
        }
    }

    public override bool ExitState()
    {
        base.ExitState();

        Debug.Log("Exit patrol state");

        return true;
    }
}