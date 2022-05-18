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


    protected virtual void OnValidate()
    {
        if (Alien == null)
            Alien = GameObject.FindGameObjectWithTag("Alien");

        if (hiveMind == null)
        {
            try
            {
                hiveMind = GameObject.FindGameObjectWithTag("Hive mind").GetComponent<HiveMind>();
            }
            catch
            {
                //Do nothing
            }
        }

        if (Alien != null && fsm == null)
            fsm = Alien.GetComponent<FiniteStateMachine>();

        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");
    }
}
