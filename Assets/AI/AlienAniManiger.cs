using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class AlienAniManiger : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    float walkingSpeed;

    [SerializeField]
    float runningSpeed;

    float walkingAniSpeed, runningAniSpeed;

    [Header("Audio files")]
    [SerializeField]
    private AudioClip[] concreteSteps;

    [SerializeField]
    private AudioClip[] swingSFX;

    [SerializeField]
    private AudioClip roarClip;

    private AudioSource audioSource;

    protected Animator ani;
    protected NavMeshAgent navMeshAgent;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        walkingAniSpeed = walkingSpeed / 3.5f;
        runningAniSpeed = runningSpeed / 3.5f;
    }

    void Update()
    {
        if (ani != null && navMeshAgent != null)
        {
            ani.SetBool("isMoving", (navMeshAgent.velocity.magnitude > 0));
        }
    }

    public void StartAttacking()
    {
        ani.SetTrigger("isAttacking");
    }

    public void StartRoar()
    {
        ani.SetTrigger("Roar");
    }

    private void Step()
    {
        AudioClip clip = concreteSteps[UnityEngine.Random.Range(0, concreteSteps.Length)];

        audioSource.volume = 0.2f;

        audioSource.PlayOneShot(clip);
    }

    private void Swing()
    {
        AudioClip clip = swingSFX[UnityEngine.Random.Range(0, swingSFX.Length)];

        audioSource.volume = 1f;

        audioSource.PlayOneShot(clip);
    }

    private void Roar()
    {
        audioSource.volume = 1f;

        audioSource.PlayOneShot(roarClip);
    }

    private void StopMovement()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
    }

    private void StartMovement()
    {
        GetComponent<NavMeshAgent>().isStopped = false;
    }


    private void IncreaseSpeed()
    {
        GetComponent<NavMeshAgent>().speed = runningSpeed;
        ani.speed = runningAniSpeed;
    }

    private void DecreaseSpeed()
    {
        GetComponent<NavMeshAgent>().speed = walkingSpeed;
        ani.speed = walkingAniSpeed;
    }
}