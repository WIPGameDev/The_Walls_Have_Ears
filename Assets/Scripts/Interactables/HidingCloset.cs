using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Audio;
using Cinemachine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class HidingCloset : Interactable
{
    private bool hiding = false;

    [SerializeField] private Transform doorTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform exteriorTransform;

    protected Animator animator;
    protected AudioSource audioSource;
    protected CinemachineVirtualCamera closetCamera;
    protected PlayerController playerController;
    protected ViewController viewController;

    [SerializeField] protected AnimationClip entryClip;
    [SerializeField] protected AnimationClip exitClip;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        closetCamera = cameraTransform.GetComponent<CinemachineVirtualCamera>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
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
        
        this.enabled = true;
        animator.SetTrigger("Hide");
    }

    public virtual void ExitCloset ()
    {
        this.enabled = false;
        hiding = false;
        animator.SetTrigger("Exit");
    }

    public void ClosetExited ()
    {
        playerController.enabled = true;
        viewController.enabled = true;
        playerController.gameObject.SetActive(true);
    }

    public void PlaySound (AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
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
        //if (playableGraph.IsValid())
        //{
        //    playableGraph.Destroy();
        //}
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
