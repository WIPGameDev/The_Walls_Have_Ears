using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
     Gizmos.color = Color.grey;

      Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
