using Assets.FSM;
using Assets.FSM.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public Dictionary<sbyte, List<PatrolPoints>> patrolPoints = new Dictionary<sbyte, List<PatrolPoints>>();

    [SerializeField]
    List<GameObject> possibleSpawnPoints = new List<GameObject>();

    [SerializeField]
    GameObject alienPrefab;

    GameObject alien;

    Animator animator;

    Dictionary<sbyte, List<AI_Sense_Base>> AISenses;

    [SerializeField]
    sbyte currentFloor;

    private void OnEnable()
    {
        sbyte[][] count = new sbyte[4][];

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Patrol point"))
        {
            PatrolPoints pp = obj.GetComponent<PatrolPoints>();

            if (!patrolPoints.ContainsKey(pp.floor))
                patrolPoints.Add(pp.floor, new List<PatrolPoints>());

            patrolPoints[pp.floor].Add(pp);
        }
    }

    private void Start()
    {
        try
        {
            alien = GameObject.FindGameObjectWithTag("Alien");
            fsm = alien.GetComponent<FiniteStateMachine>();
            animator = alien.GetComponent<Animator>();
        }
        catch { }
        try
        {
            AI_Sense_Base[] foundSenses = GameObject.FindObjectsOfType<AI_Sense_Base>();

            foreach (AI_Sense_Base sense in foundSenses)
            {
                sbyte floor = sense.floor;

                if (AISenses.ContainsKey(floor))
                    AISenses[floor].Add(sense);
                else
                {
                    AISenses.Add(floor, new List<AI_Sense_Base>());

                    AISenses[floor].Add(sense);
                }

                if (floor != currentFloor)
                    sense.enabled = false;
            }
        }
        catch { }
    }

    Vector3 DetectedLocation
    {
        set
        {
            if (fsm == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("Alien");

                if (go == null)
                {
                    GameObject closestSpawn = null;
                    float dist = -1;

                    foreach (GameObject spawn in possibleSpawnPoints)
                    {
                        float newDist = Vector3.Distance(spawn.transform.position, transform.position);

                        if (newDist > dist)
                        {
                            dist = newDist;
                            closestSpawn = spawn;
                        }
                    }

                    if (closestSpawn != null)
                    {
                        NavMeshHit hit;

                        if (NavMesh.SamplePosition(closestSpawn.transform.position, out hit, 1, 1))
                        {

                            closestSpawn.transform.position = hit.position;

                            alien = Instantiate(alienPrefab, closestSpawn.transform);

                            fsm = alien.GetComponent<FiniteStateMachine>();
                        }
                    }
                }
                else
                {
                    fsm = go.GetComponent<FiniteStateMachine>();
                }
            }

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
    }

    public void SetDetection(AISenseData Sense)
    {
        if (primarySense.ifExists == false)
        {
            primarySense = Sense;
            DetectedLocation = primarySense.Location;
            
            animator.SetTrigger("Roar");
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

    public sbyte CurrentFloor
    {
        get
        {
            return currentFloor;
        }
        set
        {
            foreach (AI_Sense_Base sense in AISenses[currentFloor])
            {
                sense.enabled = false;
            }

            currentFloor = value;

            foreach (AI_Sense_Base sense in AISenses[currentFloor])
            {
                sense.enabled = true;
            }

            alien.GetComponent<PatrolState>().floor = currentFloor;
        }
    }
}
