using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class BarrierInteractable : LockableInteractable
{
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnActivation()
    {
        animator.SetTrigger("Open");
    }

    public override void LoadSaveData(ObjectSaveData objectSaveData)
    {
        this.locked = objectSaveData.locked;
    }

    public override ObjectSaveData GetSaveData()
    {
        ObjectSaveData objectSaveData = new ObjectSaveData();
        objectSaveData.objectSceneID = this.ObjectSceneID;
        objectSaveData.locked = this.locked;
        return objectSaveData;
    }
}
