using Assets.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Sense_Base : MonoBehaviour
{
    [SerializeField]
    protected byte weight = 1;

    protected GameObject Alien;
    protected HiveMind hiveMind;
    protected FiniteStateMachine fsm;
    protected GameObject Player;

    protected virtual void Start()
    {
        if (Alien == null)
        {
            try
            {
                Alien = GameObject.FindGameObjectWithTag("Alien");
            }
            catch
            {
                print(name + " Can not find Alien");
            }
        }

        if (hiveMind == null)
        {
            try
            {
                hiveMind = GameObject.FindGameObjectWithTag("Hive mind").GetComponent<HiveMind>();
            }
            catch
            {
                //Debug.LogError(name + " Can not find hive mind");
                print(name + " Can not find hive mind");
            }
        }

        if (Alien != null && fsm == null)
        {
            try
            {
                fsm = Alien.GetComponent<FiniteStateMachine>();
            }
            catch
            {
                print(name + " Can not find fsm");
            }
        }

        if (Player == null)
        {
            try
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            catch
            {
                print(name + " Can not find player");
            }
        }
    }
}
