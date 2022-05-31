using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicAndAmbientSounds", menuName = "ScriptableObjects/MusicAndAmbientSounds", order = 3)]
public class MusicAndAmbientSounds : ScriptableObject
{
    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioClip[] ambientSounds;

    public AudioClip[] Music { get => music; }
    public AudioClip[] AmbientSounds { get => ambientSounds; }
}
