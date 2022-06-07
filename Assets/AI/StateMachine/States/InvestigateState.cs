using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.FSM.States
{
    [CreateAssetMenu(fileName = "InvestigateState", menuName = "AI FSM State/States/Investigate", order = 2)]
    class InvestigateState : AbstractFMSState
    {
        Vector3 investigativePoint;

        [SerializeField]
        sbyte numberOfSearchAreas = 3;

        [SerializeField]
        float searchTimer = 1f;

        [SerializeField]
        sbyte numberOfChecks = 0;
        sbyte curNumberOfChecks;

        [SerializeField]
        float searchDistance = 1;

        AI_EchoLocation echoLocation;

        bool ifSearchingAround;

        List<Vector3> SearchLocations = new List<Vector3>();

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
                #region Set defaults
                ifSearchingAround = false;

                SearchLocations.Clear();

                navMeshAgent.isStopped = false;

                if (echoLocation == null)
                {
                    try
                    {
                        echoLocation = fsm.gameObject.GetComponent<AI_EchoLocation>();
                        echoLocation.enabled = true;
                    }
                    catch
                    {
                        Debug.LogError(name + " Can not find echo location");
                    }
                }
                else
                    echoLocation.enabled = true;
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
                #region not searching 
                if (!ifSearchingAround)
                {
                    fsm.SetLabel(Vector3.Distance(navMeshAgent.transform.position, navMeshAgent.destination).ToString());
                    if (Vector3.Distance(navMeshAgent.transform.position, investigativePoint) < 1f)
                    {
                        fsm.SetLabel("Enters if statement");

                        ifSearchingAround = true;

                        FindSearchAreas();
                        fsm.SetLabel("Found search areas");

                        System.Random rng = new System.Random();

                        randomIndex = Convert.ToSByte(rng.Next(SearchLocations.Count()));
                    }
                    else if (navMeshAgent.destination != investigativePoint)
                    {
                        navMeshAgent.SetDestination(investigativePoint);
                    }
                }
                #endregion
                #region searching
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

                                byte loopCount = 0;

                                bool loopBreak = false;

                                do
                                {
                                    randomIndex = Convert.ToSByte(rng.Next(SearchLocations.Count()));

                                    if (++loopCount >= 32)
                                        fsm.EnterState(FSMStateType.IDLE); //FINISH THIS, have a return state or something

                                    if (randomIndex != curIndex)
                                    {
                                        navMeshAgent.isStopped = true;

                                        fsm.EnterState(FSMStateType.IDLE, searchTimer);

                                        loopBreak = true;
                                    }
                                } while (!loopBreak);
                            }
                            else
                            {
                                if (!hiveMind.NextCheck())
                                {
                                    navMeshAgent.isStopped = true;
                                    fsm.EnterState(FSMStateType.IDLE);
                                }
                            }
                        }
                    }
                }
                #endregion
            }
        }

        private void FindSearchAreas()
        {
            fsm.SetLabel("Entered function to find search areas");
            
            if (SearchLocations == null)
                fsm.SetLabel("Search locations is null");
            else
                fsm.SetLabel(SearchLocations.Count().ToString());

            #region old random locations
            //for (int i = 1; i <= numberOfSearchAreas; i++)
            //{
            //    float rotation = (360f / numberOfSearchAreas) * i;

            //    Vector3 searchPoint = ((Quaternion.Euler(0, 0, rotation) * Vector3.forward) * searchDistance) + investigativePoint;

            //    NavMeshHit hitResult;

            //    if (NavMesh.SamplePosition(searchPoint, out hitResult, searchDistance * 2, 1))
            //    {
            //        if (!SearchLocations.Contains(hitResult.position))
            //            SearchLocations.Add(hitResult.position);
            //    }
            //}
            #endregion
            #region new random locations
            System.Random rng = new System.Random();

            for (int i = 0; i < numberOfSearchAreas; i++)
            {
                Vector3 randomLocation = new Vector3(rng.Next(2, (int)searchDistance), 0, rng.Next(2, (int)searchDistance));

                randomLocation += investigativePoint;
                //fsm.SetLabel("In for loop");

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomLocation, out hit, searchDistance, 1))
                {
                    //fsm.SetLabel(randomLocation.ToString());
                    SearchLocations.Add(hit.position);
                    fsm.SetLabel("found search location");
                }
            }
            #endregion

            fsm.SetLabel("Number of search locations " + SearchLocations.Count);

            if (SearchLocations.Count != numberOfChecks)
                numberOfChecks = Convert.ToSByte(SearchLocations.Count / 1.5f);
        }

        public override bool ExitState()
        {
            base.ExitState();

            if (echoLocation != null)
                echoLocation.enabled = false;

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
                if (numberOfChecks < 2)
                    numberOfChecks = 2;
                else
                    numberOfChecks = numberOfSearchAreas;
            }
            # endregion

            if (searchTimer < 1f)
                searchTimer = 1f;

            if (echoLocation == null && fsm != null)
            {
                try
                {
                    echoLocation = fsm.gameObject.GetComponent<AI_EchoLocation>();
                }
                catch
                {
                    Debug.LogError(name + " Can not find echo location");
                }
            }
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
