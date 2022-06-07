using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class AlienAniManiger : MonoBehaviour
{

    protected Animator ani;
    protected NavMeshAgent navMeshAgent;

    void Start()
    {
        ani = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
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
}
