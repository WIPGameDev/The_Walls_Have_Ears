using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct GroundMaterial
{
    public const string Concrete = "Concrete";
    public const string Wood = "Wood";
    public const string Forest = "Leaves";
    public const string Water = "Water";
}

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(PlayerController))]
public class PlayerAudioSource : MonoBehaviour
{
    private AudioSource audioSource;
    private PlayerController playerController;

    private AudioClip clip;
    private AudioClip landingSound;
    private AudioClip[] steps;

    [SerializeField] private StepsAudioClips forestAudioClips;
    [SerializeField] private StepsAudioClips concreteAudioClips;
    [SerializeField] private StepsAudioClips woodAudioClips;
    [SerializeField] private StepsAudioClips waterAudioClips;

    [SerializeField] private float checkDistance = 2f;
    [SerializeField] private LayerMask checkMask;
    [SerializeField] private float crouchCadence = 0.75f;
    [SerializeField] private float walkCadence = 1f;
    [SerializeField] private float runCadence = 2f;
    private float cadence;
    private float timeSinceLastStep = 0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
        audioSource.clip = clip;
        cadence = walkCadence;
        timeSinceLastStep = cadence;
        steps = concreteAudioClips.Steps;
        landingSound = concreteAudioClips.LandingSound;
    }

    public void Step ()
    {
        UpdateClip();
        if (CheckPlay())
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void Land ()
    {
        timeSinceLastStep = 0f;
        audioSource.clip = landingSound;
        audioSource.Play();
    }

    public void WalkCadence ()
    {
        cadence = walkCadence;
    }

    public void RunCadence()
    {
        cadence = runCadence;
    }

    public void CrouchCadence()
    {
        cadence = crouchCadence;
    }

    private bool CheckPlay ()
    {
        if (timeSinceLastStep >= cadence)
        {
            timeSinceLastStep = 0f;
            return true;
        }
        timeSinceLastStep += Time.deltaTime;
        return false;
    }

    private void UpdateClip ()
    {
        CheckGroundTag();
        int index = Random.Range(0, steps.Length);
        clip = steps[index];
    }

    private void CheckGroundTag ()
    {
        RaycastHit result;
        if (Physics.Raycast(transform.position, transform.forward, out result, checkDistance, checkMask))
        {
            switch (result.transform.tag)
            {
                case GroundMaterial.Concrete:
                {
                        steps = concreteAudioClips.Steps;
                        landingSound = concreteAudioClips.LandingSound;
                        break;
                }
                case GroundMaterial.Water:
                {
                        steps = waterAudioClips.Steps;
                        landingSound = waterAudioClips.LandingSound;
                        break;
                }
                case GroundMaterial.Forest:
                {
                        steps = forestAudioClips.Steps;
                        landingSound = forestAudioClips.LandingSound;
                        break;
                }
                case GroundMaterial.Wood:
                {
                        steps = woodAudioClips.Steps;
                        landingSound = woodAudioClips.LandingSound;
                        break;
                }
            }
                
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Vector3.Angle(Vector3.up, hit.normal) <= 45f)
        {
            switch (hit.transform.tag)
            {
                case GroundMaterial.Concrete:
                    {
                        steps = concreteAudioClips.Steps;
                        landingSound = concreteAudioClips.LandingSound;
                        break;
                    }
                case GroundMaterial.Water:
                    {
                        steps = waterAudioClips.Steps;
                        landingSound = waterAudioClips.LandingSound;
                        break;
                    }
                case GroundMaterial.Forest:
                    {
                        steps = forestAudioClips.Steps;
                        landingSound = forestAudioClips.LandingSound;
                        break;
                    }
                case GroundMaterial.Wood:
                    {
                        steps = woodAudioClips.Steps;
                        landingSound = woodAudioClips.LandingSound;
                        break;
                    }
            }
        }
    }
}
