using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Sight : MonoBehaviour
{
    NavMeshAgent navMeshAgent;

    [SerializeField]
    float Distance = 10f;

    [SerializeField]
    float halfAngle = 10f;

    Collider[] Colliders = new Collider[10];

    [SerializeField]
    protected LayerMask detectLayer;

    [SerializeField]
    byte Segments = 4;

    [SerializeField]
    protected LayerMask occlusionLayer;

    [SerializeField]
    float scanInterval = 0.1f;

    [SerializeField]
    bool ifDebugging = true;

    float curTime = 0f;

    #region Private variablels
    GameObject Alien;
    HiveMind hiveMind;
    #endregion

    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime < scanInterval)
        {
            Scan();
            curTime = 0f;
        }
    }

    void Scan()
    {
        int count = Physics.OverlapSphereNonAlloc(gameObject.transform.position, Distance, Colliders, detectLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Colliders[i].gameObject;
            if (IsInSight(obj))
                hiveMind.DetectedLocation = obj.transform.position;
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = gameObject.transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;

        float deltaAngle = Vector3.Angle(direction, gameObject.transform.forward);

        if (deltaAngle > halfAngle * 4)
            return false;

        if (Physics.Linecast(origin, dest, occlusionLayer))
            return false;

        return true;
    }

    Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] Vertices = new Vector3[Segments + 1];

        #region Vertices
        Vertices[0] = Vector3.zero;

        Vector3 endPoint = Vector3.forward * Distance;

        int rotation =  360 / Segments;

        for (int i = 1; i <= Segments; i++)
        {
            Vertices[i] = Quaternion.Euler(0, 0, rotation * i) * Vector3.up * halfAngle + endPoint;
        }
        #endregion

        #region Triangles
        #region Code long 
        int[] Triangles = new int[(Segments + Segments - 2) * 3];
        int x = 0;
        for (int i = 1; i <= Segments; i++)
        {
            Triangles[x++] = 0;

            if (i != Segments)
                Triangles[x++] = i + 1;
            else
                Triangles[x++] = 1;

            Triangles[x++] = i;
        }
        #endregion

        #region Circle
        int k = 1;
        int count = 1;
        for (int i = 1; i <= Segments - 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Triangles[x++] = k;

                if (j < 2)
                {
                    if (k + count <= Segments)
                        k += count;
                    else
                    {
                        if (k + count - Segments == 1)
                            k = k + count++ - Segments;
                        else
                            k = k + ++count - Segments;
                    }
                }
            }
        }
        #endregion
        #endregion

        mesh.vertices = Vertices;

        mesh.triangles = Triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        if (Alien == null)
            Alien = GameObject.FindGameObjectWithTag("Alien");

        if (hiveMind == null)
            hiveMind = GameObject.FindGameObjectWithTag("Hive mind").GetComponent<HiveMind>();

        if (Segments < 3)
            Segments = 3;
        else if (Segments > 8)
            Segments = 8;
    }

    private void OnDrawGizmos()
    {
        if (ifDebugging)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawMesh(CreateMesh(), gameObject.transform.position, gameObject.transform.rotation);

            int count = Physics.OverlapSphereNonAlloc(gameObject.transform.position, Distance, Colliders, detectLayer, QueryTriggerInteraction.Collide);

            for (int i = 0; i < count; i++)
            {
                if (IsInSight(Colliders[i].gameObject))
                    Gizmos.color = new Color(0, 1, 0, 0.2f);
                else
                    Gizmos.color = new Color(1, 0, 0, 0.2f);

                float radius = Colliders[i].gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * 0.75f;

                Gizmos.DrawSphere(Colliders[i].gameObject.transform.position, radius);
            }
        }
    }
}
