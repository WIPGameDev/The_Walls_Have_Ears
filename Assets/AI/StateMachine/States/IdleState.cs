using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FSM.States
{
    [CreateAssetMenu(fileName = "IdleState", menuName = "AI FSM State/States/Idle", order = 1)]
    public class IdleState : AbstractFMSState
    {
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.IDLE;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            return EnteredState;
        }

        public override void UpdateState()
        {
        }

        public override bool ExitState()
        {
            base.ExitState();

            return true;
        }
    }

}
