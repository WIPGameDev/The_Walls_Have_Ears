using Assets.FSM;
using Assets.FSM.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveMind : MonoBehaviour
{
    FiniteStateMachine fsm;

    private void OnValidate()
    {
        if (fsm == null)
            fsm = GameObject.FindGameObjectWithTag("Alien").GetComponent<FiniteStateMachine>();
    }

    public Vector3 DetectedLocation
    {
        set
        {
            if (fsm != null)
            {
                if (fsm.CurrentState.StateType != FSMStateType.CHASE)
                {
                    if (fsm.FSMStates.ContainsKey(FSMStateType.INVESTIGATE))
                    {
                        try
                        {
                            InvestigateState InvState = fsm.FSMStates[FSMStateType.INVESTIGATE] as InvestigateState;
                            InvState.InvestigativePoint = value;

                            fsm.EnterState(InvState);
                        }
                        catch
                        {
                            Debug.LogError("Hive mind cannot find investigative state");
                        }
                    }
                    else
                        Debug.LogError("FSM Doesn't contain investigative state");
                }
                else
                    Debug.LogError("Hivemind FSM reference is null");
            }
        }
    }
}
