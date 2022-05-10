using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FSM.States
{
    [CreateAssetMenu(fileName = "AttackState", menuName = "AI FSM State/States/Attack", order = 4)]
    public class AttackState : AbstractFMSState
    {
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

            //FINISH THIS, trigger an attack (ani or whatever)

            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
                Debug.Log("Updating attack state");

            //Probably redundant but needed
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("Exit attack state");

            return true;
        }
    }
}
