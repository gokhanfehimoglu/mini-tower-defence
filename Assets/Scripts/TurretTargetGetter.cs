using System;
using System.Collections.Generic;
using UnityEngine;
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

        private List<Collider> _targets;

        private void Start()
        {
            _targets = new List<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out TargetableBehaviour target))
            {
                return;
            }

            var attackable = other.GetComponent<IAttackable>(); 
            
            if (HasTarget)
            {
                _targets.Add(other);
                attackable.GetOnDeathNotifier().AddListener(OnTargetDeath);
                return;
            }

            attackable.GetOnDeathNotifier().AddListener(OnTargetDeath);

            CurrentTarget = target;

            onTargetFound.Invoke(CurrentTarget);
        }

        private void OnTargetDeath(Collider bodyCollider)
        {
            OnTriggerExit(bodyCollider);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out TargetableBehaviour target))
            {
                return;
            }
            
            other.GetComponent<IAttackable>().GetOnDeathNotifier().RemoveListener(OnTargetDeath);
            
            if (!HasTarget) return;

            if (CurrentTarget != target)
            {
                _targets.Remove(other);
                return;
            }

            CurrentTarget = null;

            onTargetLost.Invoke();

            if (_targets.Count == 0) return;
            
            OnTriggerEnter(_targets[0]);
            _targets.RemoveAt(0);
        }
    }
}