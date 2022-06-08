using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.FSM.States
{
    [CreateAssetMenu(fileName = "AttackState", menuName = "AI FSM State/States/Attack", order = 4)]
    public class AttackState : AbstractFMSState
    {
        GameObject Player;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.ATTACK;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
                Debug.Log("Entered attack state");

            Player.SendMessage("Attacked");

            NavMeshAgent nma = fsm.gameObject.GetComponent<NavMeshAgent>();

            nma.transform.LookAt(Player.transform);

            nma.isStopped = true;

            fsm.gameObject.GetComponent<AlienAniManiger>().StartAttacking();

            fsm.enabled = false;

            navMeshAgent.gameObject.GetComponent<Animator>().speed = 1;

            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                Debug.Log("Updating attack state");


            }
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("Exit attack state");

            return true;
        }

        private void OnValidate()
        {
            if (Player == null)
                Player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
