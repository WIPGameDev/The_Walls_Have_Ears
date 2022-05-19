using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour, ISaveable
{
    [SerializeField] [HideInInspector] protected string objectSceneID = Guid.NewGuid().ToString();
    private float stayTime = 0f;
    [SerializeField] private float timelimit = 0f;

    [SerializeField] private UnityEvent entered;
    [SerializeField] private UnityEvent exited;
    [SerializeField] private UnityEvent time;

    public string ObjectSceneID { get => objectSceneID; }

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

    public string GetSaveData()
    {
        TriggerSaveData data = new TriggerSaveData();
        data.objectSceneID = this.ObjectSceneID;
        data.isActive = gameObject.activeInHierarchy;
        return JsonUtility.ToJson(data);
    }

    public void LoadSaveData(string json)
    {
        TriggerSaveData data = JsonUtility.FromJson<TriggerSaveData>(json);
        gameObject.SetActive(data.isActive);
    }

    public string GetObjectID()
    {
        return objectSceneID;
    }
}