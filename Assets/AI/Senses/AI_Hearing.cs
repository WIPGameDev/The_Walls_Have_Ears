using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Hearing : AI_Sense_Base
{
    public float Radius;
    
    Collider[] Colliders = new Collider[10];

    [SerializeField]
    protected LayerMask detectLayer;

    [SerializeField]
    float scanInterval = 0.1f;

    float curTime = 0f;

    #region Private variablels
    GameObject Alien;
    HiveMind hiveMind;
    #endregion

    private void Awake()
    {
        weight = 2;
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime < scanInterval)
        {
            Scan();
            curTime = 0f;
        }
    }

    public void Scan()
    {
        int count = Physics.OverlapSphereNonAlloc(gameObject.transform.position, Radius, Colliders, detectLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            try
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(Colliders[i].gameObject.transform.position, out hit, 10, 1))
                    hiveMind.SetDetection(new AISenseData(Colliders[i].gameObject, hit.position, weight));
                else
                    Debug.LogError(Colliders[count].gameObject.name + " couldn't find location on nav mesh");
            }
            catch
            {
                Debug.LogError("Error in hearing detection");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.5f);
        Gizmos.DrawWireSphere(gameObject.transform.position, Radius);
    }

    private void OnValidate()
    {
        if (Alien == null)
            Alien = GameObject.FindGameObjectWithTag("Alien");

        if (hiveMind == null)
            hiveMind = GameObject.FindGameObjectWithTag("Hive mind").GetComponent<HiveMind>();
    }
}
