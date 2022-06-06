using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WorldObject : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip collisionSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        audioSource.Play();
    }
}
