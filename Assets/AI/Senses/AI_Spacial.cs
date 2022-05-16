using Assets.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Spacial : AI_Sense_Base
{
    [SerializeField]
    float Radius = 2;

    [SerializeField]
    protected LayerMask detectLayer;

    Collider[] Colliders = new Collider[1];

    [SerializeField]
    float scanInterval = 0.1f;

    float curTime;

    protected FiniteStateMachine fsm;

    private void Awake()
    {
        weight = 4;
    }

    private void Start()
    {
        curTime = scanInterval;
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= scanInterval)
        {
            Scan();
            curTime = 0f;
        }
    }

    private void Scan()
    {
        int count = Physics.OverlapSphereNonAlloc(gameObject.transform.position, Radius, Colliders, detectLayer, QueryTriggerInteraction.Collide);

        if (count != 0)
        {
            fsm.EnterState(FSMStateType.CHASE);
        }
    }

    private void OnValidate()
    {
        if (Radius < 1)
            Radius = 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(gameObject.transform.position, Radius);
    }
}
