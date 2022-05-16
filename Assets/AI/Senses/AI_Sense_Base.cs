using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Sense_Base : MonoBehaviour
{
    [SerializeField]
    protected byte weight = 1;

    protected GameObject Alien;
    protected HiveMind hiveMind;

    private void OnValidate()
    {
        if (Alien == null)
            Alien = GameObject.FindGameObjectWithTag("Alien");

        if (hiveMind == null)
            hiveMind = GameObject.FindGameObjectWithTag("Hive mind").GetComponent<HiveMind>();
    }
}
