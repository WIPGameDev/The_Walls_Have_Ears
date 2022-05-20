using Assets.FSM;
using Assets.FSM.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AISenseData
{
    public bool ifExists;

    GameObject obj;

    Vector3 location;

    byte weight;

    public AISenseData(GameObject Object, Vector3 Location, byte Weight)
    {
        ifExists = true;

        obj = Object;

        location = Location;
        weight = Weight;
    }

    #region Getters
    public GameObject Object
    {
        get
        {
            return obj;
        }
    }

    public Vector3 Location
    {
        get
        {
            return location;
        }
    }

    public float Weight
    {
        get
        {
            return weight;
        }
    }
    #endregion
}

public class HiveMind : MonoBehaviour
{
    FiniteStateMachine fsm;

    AISenseData primarySense = new AISenseData();

    AISenseData secondarySense = new AISenseData();

    private void OnValidate()
    {
        if (fsm == null)
            fsm = GameObject.FindGameObjectWithTag("Alien").GetComponent<FiniteStateMachine>();
    }

    Vector3 DetectedLocation //FINISH THIS, change the way this works so that I input a weight parameter  
    {
        set
        {
            if (fsm != null)
            {
                if (fsm.CurrentState != null && fsm.CurrentState.StateType != FSMStateType.CHASE)
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
                    Debug.LogError("FSM current state is null or is already in chase");
            }
            else
                Debug.LogError("Hivemind FSM reference is null");
        }
    }

    public void SetDetection(AISenseData Sense)
    {
        if (primarySense.ifExists == false)
        {
            primarySense = Sense;
            DetectedLocation = primarySense.Location;
        }
        else
        {
            if (primarySense.Weight <= Sense.Weight)
            {
                secondarySense = primarySense;
                primarySense = Sense;
                DetectedLocation = primarySense.Location;
            }
            else
            {
                if (secondarySense.ifExists == false)
                    secondarySense = Sense;
                else if (secondarySense.Weight <= Sense.Weight)
                    secondarySense = Sense;
            }
        }
    }

    public bool NextCheck()
    {
        if (secondarySense.ifExists)
        {
            primarySense = secondarySense;

            secondarySense = new AISenseData();
            
            DetectedLocation = primarySense.Location;

            return true;
        }

        return false;
    }
}
