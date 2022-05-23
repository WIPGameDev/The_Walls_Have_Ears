using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> linkedPoints = new List<GameObject>();

    public sbyte floor = 0;

    private byte loggedCount = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawSphere(gameObject.transform.position, 0.2f);
    }

    private void OnValidate()
    {
        if (loggedCount != (byte)linkedPoints.Count)
        {
            for (int i = 0; i < linkedPoints.Count; i++)
            {
                if (i != linkedPoints.Count - 1)
                    for (int k = i + 1; k < linkedPoints.Count; k++)
                    {
                        if (linkedPoints[i] == linkedPoints[k])
                            linkedPoints.RemoveAt(k);
                    }

                if (!linkedPoints[i].GetComponent<PatrolPoints>().linkedPoints.Contains(gameObject))
                    linkedPoints[i].GetComponent<PatrolPoints>().linkedPoints.Add(gameObject);
            }
            loggedCount = (byte)linkedPoints.Count;
        }
    }
}