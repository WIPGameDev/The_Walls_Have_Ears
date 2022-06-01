using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "PatrolState", menuName = "AI FSM State/States/Patrol", order = 5)]
public class PatrolState : AbstractFMSState
{
    [SerializeField]
    sbyte floor = 0;
    
    byte index;

    [SerializeField]
    LayerMask detectLayer;

    [SerializeField]
    float MaxMovementTime = 30f;

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

            index = 255;

            float storedDist = float.MaxValue;

            for (int i = 0; i < hiveMind.patrolPoints[floor].Count; i++)
            {
                float dist = Vector3.Distance(navMeshAgent.transform.position, hiveMind.patrolPoints[floor][i].transform.position);

                if (dist < storedDist)
                {
                    storedDist = dist;
                    index = (byte)i;
                }
            }

            if (index == 255)
            {
                Debug.LogError("Failed to enter patrol state: index = 255");
                return false;
            }

            if (hiveMind.patrolPoints[floor][index].linkedPoints.Count == 0)
            {
                Debug.LogError(hiveMind.patrolPoints[floor][index].name + " does not contain any linked points");
                return false;
            }

            byte rng = (byte)Random.Range(0, hiveMind.patrolPoints[floor][index].linkedPoints.Count);

            NavMeshHit hit;

            NavMesh.SamplePosition(hiveMind.patrolPoints[floor][index].linkedPoints[rng].transform.position, 
                out hit, 10, NavMesh.AllAreas);

            navMeshAgent.SetDestination(hit.position);

            Debug.Log("Entered patrol state");
        }

        return EnteredState;
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
                fsm.EnterState(FSMStateType.IDLE, 1f);
        }
    }

    public override bool ExitState()
    {
        base.ExitState();

        Debug.Log("Exit patrol state");

        return true;
    }
}