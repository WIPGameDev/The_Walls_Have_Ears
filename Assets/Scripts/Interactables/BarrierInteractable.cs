using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class BarrierInteractable : LockableInteractable
{
    protected Animator animator;
    protected PlayableGraph playableGraph;
    protected AnimationPlayableOutput animationPlayableOutput;
    [SerializeField] protected AnimationClip clip;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        playableGraph = PlayableGraph.Create();
        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        animationPlayableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
        var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        animationPlayableOutput.SetSourcePlayable(clipPlayable);
    }

    public override void OnActivation()
    {
        playableGraph.Play();
    }

    public override void LoadSaveData(ObjectSaveData objectSaveData)
    {
        this.locked = objectSaveData.locked;
    }

    public override ObjectSaveData GetSaveData()
    {
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(animatorStateInfo.normalizedTime);
        ObjectSaveData objectSaveData = new ObjectSaveData();
        objectSaveData.objectSceneID = this.ObjectSceneID;
        objectSaveData.locked = this.locked;
        return objectSaveData;
    }

    private void OnDestroy()
    {
        playableGraph.Destroy();
    }
}
