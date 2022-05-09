using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Touch : MonoBehaviour
{
    GameObject Alien;
    HiveMind hiveMind;

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision Stay");

        if (collision.gameObject.tag == "Player")
        {
            #region hiveMind.DetectedLocation = collision.contacts[0].point;
            try
            {
                hiveMind.DetectedLocation = collision.contacts[0].point;
            }
            catch
            {
                Debug.LogError("Hive mind does not exist for AI_Touch in " + gameObject.name);
            }
            #endregion
        }
    }

    private void OnValidate()
    {
        if (Alien == null)
            Alien = GameObject.FindGameObjectWithTag("Alien");

        if (hiveMind == null)
            hiveMind = GameObject.FindGameObjectWithTag("Hive mind").GetComponent<HiveMind>();
    }
}
