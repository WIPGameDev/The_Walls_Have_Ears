using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.FSM
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class FiniteStateMachine : MonoBehaviour
    {
        AbstractFMSState currentState;

        [SerializeField] 
        List<AbstractFMSState> validStates;

        [SerializeField]
        FSMStateType DefaultState;

        Dictionary<FSMStateType, AbstractFMSState> fsmStates;

        Vector3 detectedPoint;

        public void Awake()
        {
            currentState = null;

            fsmStates = new Dictionary<FSMStateType, AbstractFMSState>();

            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();

            //Add each valid state
            foreach (AbstractFMSState state in validStates)
            {
                state.SetFSM(this);
                state.SetNavMeshAgent(navMeshAgent);
                fsmStates.Add(state.StateType, state);
            }
        }

        private void Start()
        {
            //Default state
            EnterState(DefaultState);
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.UpdateState();
            }
        }

        #region EnterState
        public void EnterState(AbstractFMSState nextState)
        {
            if (nextState == null)
                return;

            if (currentState != null)
                currentState.ExitState();

            currentState = nextState;
            currentState.EnterState();
        }
        public void EnterState(FSMStateType stateType)
        {
            if (fsmStates.ContainsKey(stateType))
            {
                AbstractFMSState nextState = fsmStates[stateType];

                EnterState(nextState);
            }
        }
        #endregion

        public Vector3 DetectedPoint
        {
            get
            {
                return detectedPoint;
            }
            set
            {
                detectedPoint = value;
            }
        }

        public AbstractFMSState CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                currentState = value;
            }
        }

        public Dictionary<FSMStateType, AbstractFMSState> FSMStates
        {
            get
            {
                return fsmStates;
            }
        }
    }
}
