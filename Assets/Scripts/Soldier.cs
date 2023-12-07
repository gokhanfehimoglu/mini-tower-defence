using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiniTowerDefence
{
    public abstract class Soldier : MonoBehaviour, IAttackable
    {
        [SerializeField]
        private float speed;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        protected float damage;

        [SerializeField]
        private float health;

        [SerializeField]
        private CapsuleCollider bodyCollider;

        protected IAttackable currentAttackable;
        
        private float _roadPercentage;
        private State _currentState;
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        private readonly UnityEvent<Collider> _onDeathNotifier = new();
        private List<Collider> _targets;

        private void Start()
        {
            _roadPercentage = GetPathway() == PathWay.Forward ? 0f : 1f;
            SetState(State.Running);
        }

        protected virtual void Update()
        {
            if (_currentState is State.Attacking or State.Idle) return;

            var soldierTransform = transform;

            if (GetPathway() == PathWay.Forward && _roadPercentage > 1f)
            {
                SetState(State.Idle);
                _roadPercentage = 1f;
            }
            else if (GetPathway() == PathWay.Backward && _roadPercentage < 0f)
            {
                SetState(State.Idle);
                _roadPercentage = 0f;
            }

            var point = RoadManager.Instance.GetRoadPoint(_roadPercentage);

            soldierTransform.position = point.pos;
            soldierTransform.rotation = point.rot;

            _roadPercentage += Time.deltaTime * speed * (GetPathway() == PathWay.Backward ? -1 : 1);
        }

        private void SetState(State state)
        {
            _currentState = state;

            switch (_currentState)
            {
                case State.Attacking:
                    animator.SetBool(Running, false);
                    animator.SetBool(Idle, false);
                    animator.SetBool(Attacking, true);
                    break;
                case State.Running:
                    animator.SetBool(Idle, false);
                    animator.SetBool(Attacking, false);
                    animator.SetBool(Running, true);
                    break;
                case State.Idle:
                    animator.SetBool(Attacking, false);
                    animator.SetBool(Running, false);
                    animator.SetBool(Idle, true);
                    break;
            }
            
            OnStateChange(_currentState);
        }

        protected virtual void OnStateChange(State newState)
        {
            
        }

        public void OnAttackAreaEnter(Collider other)
        {
            var attackable = other.GetComponent<IAttackable>();
            if (currentAttackable != null)
            {
                _targets.Add(other);
                attackable.GetOnDeathNotifier().AddListener(OnTargetDeath);
                return;
            }
            currentAttackable = attackable;
            currentAttackable.GetOnDeathNotifier().AddListener(OnTargetDeath);
            SetState(State.Attacking);
        }
        
        private void OnTargetDeath(Collider targetCollider)
        {
            OnAttackAreaExit(targetCollider);
        }

        public void MeleeAttackFinished()
        {
            currentAttackable?.Attacked(damage);
        }

        public void OnAttackAreaExit(Collider other)
        {
            var attackable = other.GetComponent<IAttackable>();
            attackable.GetOnDeathNotifier().RemoveListener(OnTargetDeath);

            if (currentAttackable != attackable)
            {
                _targets.Remove(other);
                return;
            }
            
            currentAttackable = null;
            SetState(State.Running);
        }

        public void Attacked(float receivedDamage)
        {
            health -= receivedDamage;

            if (health > 0) return;

            if (GetPathway() == PathWay.Forward)
            {
                // is enemy
                WaveManager.Instance.OnEnemyDeath();
                CurrencyManager.Instance.OnEnemyDeath();
            }
            
            _onDeathNotifier?.Invoke(bodyCollider);
            
            Destroy(gameObject);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public UnityEvent<Collider> GetOnDeathNotifier()
        {
            return _onDeathNotifier;
        }

        protected abstract PathWay GetPathway();
    }

    public enum PathWay
    {
        Forward,
        Backward
    }

    public enum State
    {
        Idle,
        Running,
        Attacking
    }
}