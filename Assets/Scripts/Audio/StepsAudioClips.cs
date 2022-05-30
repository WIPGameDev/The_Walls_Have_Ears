using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "steps", menuName = "ScriptableObjects/Steps", order = 2)]
public class StepsAudioClips : ScriptableObject
{
    [SerializeField] private string groundTag;
    [SerializeField] private AudioClip landingSound;
    [SerializeField] private AudioClip[] steps;

    public string GroundTag { get => groundTag; }
    public AudioClip LandingSound { get => landingSound; }
    public AudioClip[] Steps { get => steps; }
}
