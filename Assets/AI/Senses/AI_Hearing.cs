using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Hearing : AI_Sense_Base
{
    public float Radius;
    
    Collider[] Colliders = new Collider[10];

    [SerializeField]
    protected LayerMask detectLayer;

    [SerializeField]
    float scanInterval = 0.1f;

    float curTime = 0f;

    NavMeshAgent navAgent;

    private void Awake()
    {
        weight = 2;
        navAgent.isStopped = true;
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
                {
                    navAgent.SetDestination(hit.position);

                    if (navAgent.path.corners.Length <= 1 || CalculatePathCost(navAgent.path) < Radius)
                        hiveMind.SetDetection(new AISenseData(Colliders[i].gameObject, hit.position, weight));
                }
                else
                    Debug.Log(Colliders[count].gameObject.name + " couldn't find location on nav mesh");
            }
            catch
            {
                Debug.LogError("Error in hearing detection");
            }
        }
    }

    #region Finding nav cost
    float CalculatePathCost(NavMeshPath path)
    {
        var corners = path.corners;
        if (corners.Length < 2)
            return Mathf.Infinity;

        var hit = new NavMeshHit();
        NavMesh.SamplePosition(corners[0], out hit, 0.1f, NavMesh.AllAreas);

        var pathCost = 0.0f;
        var costMultiplier = NavMesh.GetAreaCost(IndexFromMask(hit.mask));
        int mask = hit.mask;
        var rayStart = corners[0];

        for (int i = 1; i < corners.Length; ++i)
        {
            // the segment may contain several area types - iterate over each
            while (NavMesh.Raycast(rayStart, corners[i], out hit, hit.mask))
            {
                pathCost += costMultiplier * hit.distance;
                costMultiplier = NavMesh.GetAreaCost(IndexFromMask(hit.mask));
                mask = hit.mask;
                rayStart = hit.position;
            }

            // advance to next segment
            pathCost += costMultiplier * hit.distance;

            if (pathCost > Radius || pathCost is float.NaN)
                return Radius + 1;

            costMultiplier = NavMesh.GetAreaCost(IndexFromMask(hit.mask));
            mask = hit.mask;
            rayStart = hit.position;
        }

        return pathCost;
    }

    int IndexFromMask(int mask)
    {
        for (int i = 0; i < 32; ++i)
        {
            if ((1 << i) == mask)
                return i;
        }
        return -1;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.5f);
        Gizmos.DrawWireSphere(gameObject.transform.position, Radius);
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        if (navAgent == null)
            navAgent = GetComponent<NavMeshAgent>();
    }
}
