using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField]
    float maxAge = 0.3f;

    private void Start()
    {
        Destroy(gameObject, maxAge);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75f);
        Gizmos.DrawSphere(gameObject.transform.position, 0.5f);
    }
}
