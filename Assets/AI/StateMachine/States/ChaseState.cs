using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.FSM.States
{
    [CreateAssetMenu(fileName = "ChaseState", menuName = "Practice AI/States/Chase", order = 3)]
    class ChaseState : AbstractFMSState
    {
        GameObject Player;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.CHASE;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("Entered chase state");

                Debug.Log("Add distance check, and then trigger kill player");

                EnteredState = true;
            }

            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                Debug.Log("Updating chase state");

                navMeshAgent.SetDestination(Player.transform.position);
            }
        }


        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("Exit chase state");

            return true;
        }

        private void OnValidate()
        {
            if (Player == null)
                Player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
