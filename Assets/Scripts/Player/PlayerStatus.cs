using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour
{
    private PlayerController playerController;
    private bool isPlayerAlive = true;
    [SerializeField] private GameObject head;

    [Header("Events")]
    [SerializeField] private UnityEvent playerDead;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public bool IsPlayerAlive { get => isPlayerAlive; set => isPlayerAlive = value; }

    public void Attacked ()
    {
        if (isPlayerAlive)
        {
            isPlayerAlive = false;
            playerController.DisablePlayer();
            head.transform.parent = null;
            CapsuleCollider cap = head.AddComponent<CapsuleCollider>();
            cap.radius = 0.3f;
            cap.height = 0.75f;
            head.AddComponent<Rigidbody>();
            playerDead.Invoke();
        }
    }
}
