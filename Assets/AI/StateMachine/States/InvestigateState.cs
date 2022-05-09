using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.FSM.States
{
    [CreateAssetMenu(fileName = "InvestigateState", menuName = "Practice AI/States/Investigate", order = 2)]
    class InvestigateState : AbstractFMSState
    {
        Vector3 investigativePoint;

        [SerializeField]
        sbyte numberOfSearchAreas = 3;

        [SerializeField]
        sbyte numberOfChecks = 0;
        sbyte curNumberOfChecks;

        [SerializeField]
        float searchDistance = 1;

        bool ifSearchingAround;

        List<Vector3> SearchLocations;

        sbyte randomIndex;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.INVESTIGATE;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("Entered investigative state");

                #region Set defaults
                ifSearchingAround = false;

                SearchLocations.Clear();

                navMeshAgent.isStopped = false;
                #endregion

                curNumberOfChecks = 0;

                navMeshAgent.SetDestination(investigativePoint);

                EnteredState = true;
            }

            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                Debug.Log("Updating investigative state");

                if (!ifSearchingAround)
                {
                    if (Vector3.Distance(navMeshAgent.transform.position, investigativePoint) < 1f)
                    {
                        ifSearchingAround = true;

                        FindSearchAreas();

                        System.Random rng = new System.Random();

                        randomIndex = Convert.ToSByte(rng.Next(SearchLocations.Count()));
                    }
                    else if (navMeshAgent.destination != investigativePoint)
                    {
                        navMeshAgent.SetDestination(investigativePoint);
                    }
                }
                else
                {
                    if (navMeshAgent.destination != SearchLocations[randomIndex])
                    {
                        if (curNumberOfChecks++ < numberOfChecks)
                            navMeshAgent.SetDestination(SearchLocations[randomIndex]);
                    }
                    else
                    {
                        if (Vector3.Distance(navMeshAgent.transform.position, navMeshAgent.destination) < 1f)
                        {
                            if (curNumberOfChecks < numberOfChecks)
                            {
                                sbyte curIndex = randomIndex;
                                System.Random rng = new System.Random();
                                do
                                {
                                    randomIndex = Convert.ToSByte(rng.Next(SearchLocations.Count()));
                                } while (randomIndex == curIndex);
                            }
                            else
                            {
                                navMeshAgent.isStopped = true;
                                fsm.EnterState(FSMStateType.IDLE);
                            }
                        }
                    }
                }
            }
        }

        private void FindSearchAreas()
        {
            for (int i = 1; i <= numberOfSearchAreas; i++)
            {
                float rotation = (360f / numberOfSearchAreas) * i;

                Vector3 searchPoint = (Quaternion.Euler(0, rotation, 0) * investigativePoint) * searchDistance;

                NavMeshHit hitResult;

                if (NavMesh.SamplePosition(searchPoint, out hitResult, searchDistance * 2, 1))
                {
                    if (!SearchLocations.Contains(hitResult.position))
                        SearchLocations.Add(hitResult.position);
                }
            }

            Debug.Log("Number of search locations " + SearchLocations.Count);

            if (SearchLocations.Count != numberOfChecks)
                numberOfChecks = Convert.ToSByte(SearchLocations.Count / 1.5f);
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("Exit investigative state");

            return true;
        }

        private void OnValidate()
        {
            #region Search areas
            if (numberOfSearchAreas < 3)
                numberOfSearchAreas = 3;
            else if (numberOfSearchAreas > 8)
                numberOfSearchAreas = 8;
            #endregion

            #region Number of checks
            if (numberOfChecks > numberOfSearchAreas / 1.5f)
                numberOfChecks = Convert.ToSByte(numberOfSearchAreas / 1.5f);
            else if (numberOfChecks < numberOfSearchAreas && numberOfChecks < 1)
            {
                if (numberOfChecks < 1)
                    numberOfChecks = 1;
                else
                    numberOfChecks = numberOfSearchAreas;
            }
            #endregion
        }

        public Vector3 InvestigativePoint
        {
            get
            {
                return investigativePoint;
            }
            set
            {
                investigativePoint = value;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(navMeshAgent.destination, 100f);
        }
    }
}
