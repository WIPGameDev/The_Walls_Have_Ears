using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class HidingCloset : Interactable
{
    private bool hiding = false;

    [SerializeField] private Transform doorTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform exteriorTransform;

    protected Animator animator;
    protected PlayableGraph playableGraph;

    [SerializeField] protected AnimationClip entryClip;
    [SerializeField] protected AnimationClip exitClip;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        playableGraph = PlayableGraph.Create();
        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
    }

    public override void OnActivation()
    {
        hiding = true;
        StartCoroutine(Hiding());

    }

    private IEnumerator Hiding()
    {
        Coroutine coroutine = StartCoroutine(PositionPlayer());
        yield return coroutine;
        AnimationPlayableUtilities.PlayClip(GetComponent<Animator>(), entryClip, out playableGraph);
    }

    private IEnumerator PositionPlayer ()
    {
        Transform player = FindObjectOfType<PlayerController>().transform;
        if (player != null)
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void OnDestroy()
    {
        playableGraph.Destroy();
    }
}
