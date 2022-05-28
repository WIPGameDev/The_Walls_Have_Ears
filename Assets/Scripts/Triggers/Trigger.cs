using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour, ISaveable
{
    protected GameController gameController;
    [SerializeField] [HideInInspector] protected string objectSceneID = Guid.NewGuid().ToString();
    private float stayTime = 0f;
    [SerializeField] private float timelimit = 0f;

    [SerializeField] private UnityEvent entered;
    [SerializeField] private UnityEvent exited;
    [SerializeField] private UnityEvent time;

    public string ObjectSceneID { get => objectSceneID; }

    protected virtual void Start ()
    {
        gameController = FindObjectOfType<GameController>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        stayTime = 0f;
        entered.Invoke();
    }

    protected virtual void OnTriggerStay(Collider other)
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

    protected virtual void OnTriggerExit(Collider other)
    {
        exited.Invoke();
    }

    public virtual ObjectSaveData GetSaveData()
    {
        ObjectSaveData data = new ObjectSaveData();
        data.objectSceneID = this.ObjectSceneID;
        data.isActive = gameObject.activeInHierarchy;
        return data;
    }

    public virtual void LoadSaveData(ObjectSaveData objectSaveData)
    {
        gameObject.SetActive(objectSaveData.isActive);
    }

    public void SaveCheckPoint ()
    {
        gameController.SaveCheckPoint();
    }
}