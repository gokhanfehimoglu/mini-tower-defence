using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MiniTowerDefence.StateMachine
{
    public class State : MonoBehaviour, IState
    {
        [SerializeField]
        private List<StateTransition> transitions = new();
        
        [SerializeField]
        private UnityEvent onEnter = new();
        
        [SerializeField]
        private UnityEvent onExit = new();

        public IState Transition()
        {
            return (from transition in transitions where transition.ShouldTransition() select transition.NextState)
                .FirstOrDefault();
        }

        public void Enter() => onEnter?.Invoke();

        public void Exit() => onExit?.Invoke();
    }
}