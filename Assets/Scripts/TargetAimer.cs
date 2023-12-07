using UnityEngine;

namespace MiniTowerDefence
{
    public class TargetAimer : MonoBehaviour
    {
        [SerializeField]
        private float smoothing = 0.1f;

        [SerializeField]
        private Transform spawnPoint;

        private bool _resetting = true;
        private bool _hasTarget;
        private Transform _currentTarget;
        private Quaternion _initialRotation;
        private Quaternion _targetRotation;

        private void Start()
        {
            _initialRotation = transform.rotation;
        }

        private void LateUpdate()
        {
            Aim();
        }

        public void SetTarget(Transform target)
        {
            _resetting = false;
            _currentTarget = target;
            _hasTarget = true;
        }

        public void ClearTarget()
        {
            _currentTarget = null;
            _resetting = true;
            _hasTarget = false;
        }

        public void SetTarget(TargetableBehaviour target) => SetTarget(target.TargetPoint);

        private void Aim()
        {
            if (!_hasTarget)
            {
                if (!_resetting)
                {
                    return;
                }

                _targetRotation = _initialRotation;
            }
            else
            {
                var currentTargetPosition = _currentTarget.position;
                currentTargetPosition.y = spawnPoint.position.y;
                _currentTarget.position = currentTargetPosition;
                
                _targetRotation = _resetting
                    ? _initialRotation
                    : Quaternion.LookRotation(_currentTarget.position - spawnPoint.position);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * smoothing);
        }
    }
}