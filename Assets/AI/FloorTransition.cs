using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTransition : MonoBehaviour
{
    sbyte upperFloor;
    [SerializeField]
    sbyte bottomFloor;

    Vector3 topCenter;
    Vector3 bottomCenter;

    HiveMind hiveMind;

    Collider[] colliders = new Collider[1];

    [SerializeField]
    protected LayerMask detectLayer;

    private void Start()
    {
        topCenter = transform.position + new Vector3(0f, transform.localScale.y / 2, 0);
        bottomCenter = transform.position - new Vector3(0f, transform.localScale.y / 2, 0);

        hiveMind = GameObject.FindGameObjectWithTag("Hive mind").GetComponent<HiveMind>(); 
    }

    private void Update()
    {
        if (hiveMind == null)
            this.enabled = false;

        int x = Physics.OverlapBoxNonAlloc(topCenter, transform.localScale, colliders, new Quaternion(), detectLayer);

        if (x == 0)
            x = Physics.OverlapBoxNonAlloc(bottomCenter, transform.localScale, colliders, new Quaternion(), detectLayer);
        else
            hiveMind.CurrentFloor = upperFloor;

        if (x != 0)
            hiveMind.CurrentFloor = bottomFloor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireCube(transform.position + new Vector3(0f, transform.localScale.y / 2, 0), transform.localScale);

        Gizmos.DrawWireCube(transform.position - new Vector3(0f, transform.localScale.y / 2, 0), transform.localScale);
    }

    private void OnValidate()
    {
        upperFloor = (sbyte)(bottomFloor + 1);
    }
}
