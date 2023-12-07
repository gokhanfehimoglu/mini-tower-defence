using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniTowerDefence.StateMachine
{
    [Serializable]
    public class StateTransition
    {
        [SerializeField]
        private State nextState;

        [SerializeField]
        private List<Condition> conditions = new();

        public State NextState => nextState;

        public bool ShouldTransition()
        {
            return conditions.All(condition => condition.IsMet());
        }
    }
}