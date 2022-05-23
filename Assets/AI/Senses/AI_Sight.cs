using UnityEngine;
using UnityEngine.AI;

public class AI_Sight : AI_Sense_Base
{
    NavMeshAgent navMeshAgent;

    [SerializeField]
    float Distance = 10f;

    [SerializeField]
    float halfAngle = 10f;

    [SerializeField]
    bool ifDebugging = false;

    [SerializeField]
    Color DebugColour = new Color(1, 0, 0, 0.5f);

    Collider[] Colliders = new Collider[10];

    [SerializeField]
    protected LayerMask detectLayer;

    [SerializeField]
    byte Segments = 4;

    [SerializeField]
    protected LayerMask occlusionLayer; 

    float scanTimer = 0f;

    [SerializeField]
    float scanInterval = 0.1f;

    float blindTimer = 0f;

    bool ifBlinded = false;

    VisionCone vc;

    [Header("Turning variables")]
    public float turnTime = 1f;
    public float rotationHalfAngle = 15;
    public float pauseTime = 0.5f;
    private bool ifInversedRotation = false;
    private float rotTimer;
    private bool ifRotWait = false;
    private float rotWaitTimer;
    private Quaternion defaultRotation;

    private void OnEnable()
    {
        vc.enabled = true;
        
        weight = 3;
    }

    private void OnDisable()
    {
        vc.enabled = false;
    }

    private void Start()
    {
        defaultRotation = gameObject.transform.rotation;
    }

    private void Update()
    {
        if (!ifBlinded)
        {
            scanTimer += Time.deltaTime;
            if (scanTimer >= scanInterval)
            {
                Scan();
                scanTimer = 0f;
            }

            if (!ifRotWait)
            {
                rotTimer += Time.deltaTime;
                
                if (rotTimer >= turnTime)
                {
                    rotTimer = 0f;

                    ifInversedRotation = !ifInversedRotation;

                    ifRotWait = true;
                }
                else
                {
                    float y = (Time.deltaTime / turnTime) * rotationHalfAngle * 2 *
                         ((ifInversedRotation) ? -1 : 1);

                    gameObject.transform.Rotate(Vector3.up, y);
                }
            }
            else
            {
                rotWaitTimer += Time.deltaTime;

                if (rotWaitTimer >= pauseTime)
                {
                    ifRotWait = false;

                    rotWaitTimer = 0;
                }
            }
        }
    }

    void Scan()
    {
        int count = Physics.OverlapSphereNonAlloc(gameObject.transform.position, Distance, Colliders, detectLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Colliders[i].gameObject;
            if (IsInSight(obj))
                hiveMind.SetDetection(new AISenseData(obj, obj.transform.position, weight));
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

    protected override void OnValidate()
    {
        base.OnValidate();

        if (vc == null)
            vc = GetComponentInChildren<VisionCone>();

        if (Segments < 3)
            vc.Segments = Segments = 3;
        //else if (Segments > 8)
        //    vc.Segments = Segments = 8;
        else
            vc.Segments = Segments;

        vc.Distance = Distance;

        vc.HalfAngle = halfAngle;
    }

    private void OnDrawGizmos()
    {
        if (ifDebugging)
        {
            if (!Application.isPlaying)
            {
                Gizmos.color = DebugColour;
                Gizmos.DrawMesh(vc.CreateMesh(), gameObject.transform.position, gameObject.transform.rotation);
            }


            int count = Physics.OverlapSphereNonAlloc(gameObject.transform.position, Distance, Colliders, detectLayer, QueryTriggerInteraction.Collide);

            for (int i = 0; i < count; i++)
            {
                if (IsInSight(Colliders[i].gameObject))
                    Gizmos.color = new Color(0, 1, 0, 0.2f);
                else
                    Gizmos.color = new Color(1, 0, 0, 0.2f);

                float radius = Colliders[i].gameObject.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * 0.75f;

                Gizmos.DrawSphere(Colliders[i].gameObject.transform.position, radius);
            }
        }
    }

    public void SetBlind(bool Blind)
    {
        ifBlinded = Blind;
    }
    public void SetBlind(float Duration)
    {

    }
}
