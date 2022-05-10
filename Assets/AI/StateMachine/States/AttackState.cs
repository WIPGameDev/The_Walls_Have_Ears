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
        public override void UpdateState()
        {
            throw new NotImplementedException();
        }
    }
}
