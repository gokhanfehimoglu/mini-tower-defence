﻿using UnityEngine;
using UnityEngine.Events;

namespace MiniTowerDefence.StateMachine
{
    public class TurretTargetGetter : MonoBehaviour
    {
        [SerializeField]
        private UnityTargetableEvent onTargetFound = new();

        [SerializeField]
        private UnityEvent onTargetLost = new();

        public TargetableBehaviour CurrentTarget { get; private set; }
        public bool HasTarget => CurrentTarget != null;

        private void OnTriggerEnter(Collider other)
        {
            if (HasTarget) return;

            if (!other.TryGetComponent(out TargetableBehaviour target))
            {
                return;
            }

            CurrentTarget = target;

            onTargetFound.Invoke(CurrentTarget);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!HasTarget) return;

            if (!other.TryGetComponent(out TargetableBehaviour target))
            {
                return;
            }

            if (CurrentTarget != target) return;

            CurrentTarget = null;

            onTargetLost.Invoke();
        }
    }
}