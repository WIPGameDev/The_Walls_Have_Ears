using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class BarrierInteractable : LockableInteractable
{
    protected PlayableGraph playableGraph;
    protected AnimationPlayableOutput animationPlayableOutput;
    [SerializeField] protected AnimationClip clip;

    protected override void Start()
    {
        base.Start();
        playableGraph = PlayableGraph.Create();
        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        animationPlayableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
        var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        animationPlayableOutput.SetSourcePlayable(clipPlayable);
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

    public override void OnActivation()
    {
        playableGraph.Play();
    }
}
