using Assets.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum ExecutionState
{
    NONE,
    ACTIVE,
    COMPLETED,
    TERMINATED,
};

public enum FSMStateType
{
    IDLE,
    INVESTIGATE,
    CHASE,
};

public abstract class AbstractFMSState : ScriptableObject
{
    protected NavMeshAgent navMeshAgent;
    protected FiniteStateMachine fsm;

    public ExecutionState ExecutionState { get; protected set; }
    public FSMStateType StateType { get; protected set; }
    public bool EnteredState { get; protected set; }

    public virtual void OnEnable()
    {
        ExecutionState = ExecutionState.NONE;
    }

    public virtual bool EnterState()
    {
        ExecutionState = ExecutionState.ACTIVE;

        return (navMeshAgent != null);
    }

    public abstract void UpdateState();

public virtual bool ExitState()
    {
        ExecutionState = ExecutionState.COMPLETED;

        return true;
    }

    public virtual void SetNavMeshAgent(NavMeshAgent _navMeshAgent)
    {
        if (_navMeshAgent != null)
        {
            navMeshAgent = _navMeshAgent;
        }
    }

    public virtual void SetFSM(FiniteStateMachine _fsm)
    {
        if (_fsm != null)
        {
            fsm = _fsm;
        }
    }
}
