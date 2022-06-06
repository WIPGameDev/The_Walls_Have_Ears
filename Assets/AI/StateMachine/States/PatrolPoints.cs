using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public void OnDestroy()
    {
        for (int i = 0; i < linkedPoints.Count; i++)
        {
            linkedPoints[i].GetComponent<PatrolPoints>().linkedPoints.Remove(gameObject);
        }
    }

    ~PatrolPoints()
    {
        foreach (GameObject go in linkedPoints)
        {
            PatrolPoints pp = go.GetComponent<PatrolPoints>();

            pp.linkedPoints.Remove(gameObject);
        }
    }

    private void OnValidate()
    {
        if (loggedCount != (byte)linkedPoints.Count)
        {
            foreach (GameObject go in linkedPoints)
            {
                if (go != null)
                {
                    PatrolPoints pp = go.GetComponent<PatrolPoints>();

                    if (!pp.linkedPoints.Contains(gameObject))
                    {
                        pp.linkedPoints.Add(gameObject);
                    }
                }
                else
                    linkedPoints.Remove(go);
            }

            loggedCount = (byte)linkedPoints.Count;
        }


        //if (loggedCount != (byte)linkedPoints.Count)
        //{
        //    for (int i = 0; i < linkedPoints.Count; i++)
        //    {
        //        if (i != linkedPoints.Count - 1)
        //            for (int k = i + 1; k < linkedPoints.Count; k++)
        //            {
        //                if (linkedPoints[i] == linkedPoints[k])
        //                    linkedPoints.RemoveAt(k);
        //            }

        //        if (!linkedPoints[i].GetComponent<PatrolPoints>().linkedPoints.Contains(gameObject))
        //            linkedPoints[i].GetComponent<PatrolPoints>().linkedPoints.Add(gameObject);
        //    }
        //    loggedCount = (byte)linkedPoints.Count;
        //}
    }
}

//[CustomEditor(typeof(PatrolPoints))]
//public class PatrolPointsEditor : Editor
//{

//    SerializedProperty _linkedPoints;

//    List<GameObject> linkedPoints = new List<GameObject>();

//    public void OnDestroy() //FINISH THIS, seems to call when deselected
//    {
//        foreach (GameObject go in Selection.gameObjects)
//        {
//            PatrolPoints pp = go.GetComponent<PatrolPoints>();

//            foreach (GameObject go2 in pp.linkedPoints)
//            {
//                PatrolPoints pp2 = go2.GetComponent<PatrolPoints>();

//                pp2.linkedPoints.Remove(go);
//            }
//        }
//    }
//}