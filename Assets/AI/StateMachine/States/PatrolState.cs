using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "PatrolState", menuName = "AI FSM State/States/Patrol", order = 5)]
public class PatrolState : AbstractFMSState
{
    [SerializeField]
    public sbyte floor = 0;
    
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

        if (hiveMind.patrolPoints.Count == 0)
        {
            Debug.LogError("No patrol points are present in hivemind");
            return false;
        }

        if (EnteredState)
        {

            globalIndex = 255;

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
        }

        return EnteredState;
    }

    public override bool ReEnterState(AbstractFMSState state)
    {
        localIndex = 255;

        NavMeshHit hit;
        Animator anim = new Animator();

        List<PatrolPoints> curFloorPatrolPoints = hiveMind.patrolPoints[floor];

        if (previousIndex == 255)
        {
            System.Random rng = new System.Random();
            localIndex = (byte)rng.Next(curFloorPatrolPoints[globalIndex].linkedPoints.Count);
        }
        else
        {
            if (curFloorPatrolPoints[globalIndex].linkedPoints.Count == 1)
            {
                localIndex = 0;
            }
            else
            {
                if (curFloorPatrolPoints[globalIndex].linkedPoints.Count == 2)
                {
                    localIndex = 0;

                    if (curFloorPatrolPoints[previousIndex] == curFloorPatrolPoints[globalIndex].linkedPoints[0].GetComponent<PatrolPoints>())
                        localIndex = 1;
                }
                else
                {
                    System.Random rng = new System.Random();

                    if (previousIndex != 255)
                    {
                        byte foundIndex = 255;

                        foundIndex = (byte)curFloorPatrolPoints[globalIndex].linkedPoints.IndexOf(curFloorPatrolPoints[previousIndex].gameObject);
                        do
                        {
                            localIndex = (byte)rng.Next(curFloorPatrolPoints[globalIndex].linkedPoints.Count);
                        } while (localIndex == foundIndex);

                    }
                    else
                        localIndex = (byte)rng.Next(curFloorPatrolPoints[globalIndex].linkedPoints.Count);
                }
            }
        }

        previousIndex = globalIndex;

        NavMesh.SamplePosition(curFloorPatrolPoints[globalIndex].linkedPoints[localIndex].transform.position,
               out hit, 10, NavMesh.AllAreas);
        navMeshAgent.SetDestination(hit.position);

        globalIndex = (byte)curFloorPatrolPoints.IndexOf(curFloorPatrolPoints[globalIndex].linkedPoints[localIndex].GetComponent<PatrolPoints>());

        timeMoving = 0;

        return true;
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
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

        return true;
    }
}