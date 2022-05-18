using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_EchoLocation : AI_Sense_Base
{
    [SerializeField]
    float scanInterval = 1f;

    float curTime;

    bool? numScan = null; //FINISH THIS, Maybe too fancy

    bool ifClicking = false;

    Vector3 loggedLocation = new Vector3();

    private void Awake()
    {
        weight = 4;
    }

    private void Start()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        this.enabled = false;
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if (!ifClicking)
        {
            if (curTime >= scanInterval)
            {
                Debug.Log("Start scan");

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
        //Set defaults
        ifClicking = false;

        numScan = null;
    }

    void Scan()
    {
        if (!Physics.Linecast(gameObject.transform.position, Player.transform.position))
            return;

        if (numScan is null)
        {
            if (Player != null)
            {
                loggedLocation = Player.transform.position;

                numScan = false;
            }
            else
            {
                Debug.LogError("Player does not exist within " + gameObject.name);

                this.enabled = false;
            }
        }
        else if (numScan == false)
        {
            CheckIfMoved();

            numScan = true;
        }
        else
        {
            CheckIfMoved();

            numScan = null;

            ifClicking = false;

            Debug.Log("End scan");
        }
    }

    void CheckIfMoved()
    {
        try
        {
            if (loggedLocation != Player.transform.position)
            {
                Debug.Log("End scan");

                numScan = null;

                ifClicking = false;

                fsm.EnterState(FSMStateType.CHASE);

                this.enabled = false;
            }
        }
        catch
        {
            Debug.LogError("Error in checking if player has moved in " + gameObject.name);
        }
    }
}
