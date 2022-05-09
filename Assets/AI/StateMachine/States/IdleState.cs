using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FSM.States
{
    [CreateAssetMenu(fileName = "IdleState", menuName = "Practice AI/States/Idle", order = 1)]
    public class IdleState : AbstractFMSState
    {
        [SerializeField]
        float idleDuration = 3f;
        
        float totalDuration;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.IDLE;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("Entered idle state");
                totalDuration = 0f;
            }

            EnteredState = true;
            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
               totalDuration += Time.deltaTime;

                Debug.Log("Updating idle state");

                //if (totalDuration >= idleDuration)
                //    _fsm.EnterState(FSMStateType.PATROL);
            }
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("Exit idle state");

            return true;
        }
    }

}
