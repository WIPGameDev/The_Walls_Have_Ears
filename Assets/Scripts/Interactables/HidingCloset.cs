using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using Cinemachine;

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
        PlayerController playerController = FindObjectOfType<PlayerController>();
        ViewController viewController = FindObjectOfType<ViewController>();
        playerController.enabled = false;
        viewController.enabled = false;
        CinemachineVirtualCamera closetCamera = cameraTransform.GetComponent<CinemachineVirtualCamera>();
        
        Transform playerTransform = playerController.transform;
        Transform viewTransform = viewController.transform;
        AnimationPlayableUtilities.PlayClip(GetComponent<Animator>(), entryClip, out playableGraph);
    }

    private void OnDestroy()
    {
        playableGraph.Destroy();
    }
}
