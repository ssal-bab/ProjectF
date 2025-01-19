using System.Collections.Generic;
using UnityEngine;

namespace H00N.FSM
{
    public class FSMTransitionState : FSMTransition
    {
        [SerializeField] FSMState targetState = null;

        public override bool Transition()
        {
            brain.ChangeState(targetState);
            return true;
        }
    }
}