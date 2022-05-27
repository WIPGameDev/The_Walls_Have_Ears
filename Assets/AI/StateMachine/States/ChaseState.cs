using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.FSM.States
{
    [CreateAssetMenu(fileName = "ChaseState", menuName = "AI FSM State/States/Chase", order = 3)]
    class ChaseState : AbstractFMSState
    {
        GameObject Player;

        float AttackRange = 2f;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.CHASE;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
                Debug.Log("Entered chase state");

            return EnteredState;
        }

        public override void UpdateState()
        {
            if (Player == null)
                Player = GameObject.FindGameObjectWithTag("Player");

            if (EnteredState)
            {
                Debug.Log("Updating chase state");

                navMeshAgent.SetDestination(Player.transform.position);

                //FINISH THIS, we can make this more dynamic by including the velocity 
                if (Vector3.Distance(navMeshAgent.transform.position, navMeshAgent.destination) < AttackRange)
                {
                    fsm.EnterState(FSMStateType.ATTACK);
                }
            }
        }


        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("Exit chase state");

            return true;
        }
    }
}
