using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_EchoLocation : AI_Sense_Base
{
    [SerializeField]
    float scanInterval = 1f;

    float curTime;

    bool ifClicking = false;
    protected override void Start()
    {
        base.Start();

        this.enabled = false;

        weight = 4;
    }

    private void Update()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        curTime += Time.deltaTime;
        if (!ifClicking)
        {
            if (curTime >= scanInterval)
            {
                ifClicking = true;

                curTime = 0;
            }
        }
        else
        {
            if (curTime >= 0.3f) //FINISH THIS, align with sound instead
            {
                Scan();

                curTime = 0;
            }
        }
    }

    private void OnEnable()
    {
        ifClicking = false;
    }

    void Scan()
    {
        RaycastHit hit;
        if (Physics.Linecast(gameObject.transform.position, Player.transform.position, out hit))
        {
            if (hit.collider.gameObject != Player && hit.collider.gameObject != gameObject)
                return;

            ifClicking = false;

            fsm.EnterState(FSMStateType.CHASE);

            this.enabled = false;
        }
    }
}
