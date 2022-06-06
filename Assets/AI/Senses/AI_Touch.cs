using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Touch : AI_Sense_Base
{
    private void Awake()
    {
        weight = 1;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collision Stay");

        if (other.gameObject.tag == "Player")
        {
            #region Detect location
            try
            {
                hiveMind.SetDetection(new AISenseData(other.gameObject, other.transform.position, weight));
            }
            catch
            {
                Debug.LogError("Hive mind does not exist for AI_Touch in " + gameObject.name);
            }
            #endregion
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    Debug.Log("Collision Stay");

    //    if (collision.gameObject.tag == "Player")
    //    {
    //        #region Detect location
    //        try
    //        {
    //            hiveMind.SetDetection(new AISenseData(collision.gameObject, collision.contacts[0].point, weight));
    //        }
    //        catch
    //        {
    //            Debug.LogError("Hive mind does not exist for AI_Touch in " + gameObject.name);
    //        }
    //        #endregion
    //    }
    //}
}
