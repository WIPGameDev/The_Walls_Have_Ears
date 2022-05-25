using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
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

        #region return values
        bool ifToReturn = false;

        float returnTime;

        float returnTimer;

        AbstractFMSState previousState;
        #endregion

        TextMeshPro tMesh;

        public void Awake()
        {
            currentState = null;

            fsmStates = new Dictionary<FSMStateType, AbstractFMSState>();

            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();

            HiveMind hiveMind = GameObject.FindGameObjectWithTag("Hive mind").GetComponent<HiveMind>();

            //Add each valid state
            foreach (AbstractFMSState state in validStates)
            {
                state.SetFSM(this);
                state.SetNavMeshAgent(navMeshAgent);
                state.SetHiveMind(hiveMind); 
                fsmStates.Add(state.StateType, state);
            }

            tMesh = GetComponentInChildren<TextMeshPro>();
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

                if (ifToReturn)
                {
                    returnTimer += Time.deltaTime;

                    if (returnTimer >= returnTime)
                    {
                        ifToReturn = false;

                        returnTime = returnTimer = 0;

                        EnterState(previousState);
                    }
                }
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
        public void EnterState(FSMStateType stateType, float TimeToReturn)
        {
            if (fsmStates.ContainsKey(stateType))
            {
                previousState = currentState;

                AbstractFMSState nextState = fsmStates[stateType];

                EnterState(nextState);

                ifToReturn = true;
                
                returnTime = TimeToReturn;
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

        public void SetLabel(string text)
        {
            tMesh.text = text;
        }
    }
}
