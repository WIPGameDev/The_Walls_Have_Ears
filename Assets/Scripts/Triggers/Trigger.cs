using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    private float stayTime = 0f;
    [SerializeField] private float timelimit = 0f;

    [SerializeField] private UnityEvent entered;
    [SerializeField] private UnityEvent exited;
    [SerializeField] private UnityEvent time;

    void OnTriggerEnter(Collider other)
    {
        stayTime = 0f;
        entered.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        if (timelimit > 0f)
        {
            stayTime += Time.deltaTime;
            if (stayTime >= timelimit)
            {
                time.Invoke();
                stayTime = 0f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        exited.Invoke();
    }
}