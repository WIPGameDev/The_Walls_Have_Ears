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
    protected CinemachineVirtualCamera closetCamera;
    protected PlayerController playerController;
    protected ViewController viewController;

    [SerializeField] protected AnimationClip entryClip;
    [SerializeField] protected AnimationClip exitClip;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        closetCamera = cameraTransform.GetComponent<CinemachineVirtualCamera>();
    }

    protected override void Start()
    {
        base.Start();
        playableGraph = PlayableGraph.Create();
        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
    }

    public override void OnActivation()
    {
        EnterCloset();
    }

    public virtual void EnterCloset ()
    {
        hiding = true;
        playerController = FindObjectOfType<PlayerController>();
        viewController = FindObjectOfType<ViewController>();
        playerController.enabled = false;
        viewController.enabled = false;

        Transform playerTransform = playerController.transform;
        Transform viewTransform = viewController.transform;

        playerTransform.gameObject.SetActive(false);
        playerTransform.SetPositionAndRotation(exteriorTransform.position, exteriorTransform.rotation);
        viewTransform.rotation = Quaternion.identity;

        AnimationPlayableUtilities.PlayClip(GetComponent<Animator>(), entryClip, out playableGraph);
        this.enabled = true;
    }

    public virtual void ExitCloset ()
    {
        this.enabled = false;
        hiding = false;
        AnimationPlayableUtilities.PlayClip(GetComponent<Animator>(), exitClip, out playableGraph);
        playerController.enabled = true;
        viewController.enabled = true;
        playerController.gameObject.SetActive(true);
    } 

    private void Update()
    {
        if (hiding)
        {
            if (Input.GetButtonDown("Interact"))
            {
                ExitCloset();
            }
        }
    }

    private void DestroyGraph ()
    {
        playableGraph.Destroy();
    }

    private void OnDisable()
    {
        DestroyGraph();
    }

    private void OnDestroy()
    {
        DestroyGraph();
    }
}
