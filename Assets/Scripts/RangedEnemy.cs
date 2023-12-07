using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace MiniTowerDefence
{
    public class RangedEnemy : Enemy
    {
        [SerializeField]
        private float fireRate;

        [SerializeField]
        private Transform firePoint;
        
        [SerializeField]
        private Vector3 bulletSpeed;

        [SerializeField]
        private Bullet bulletPrefab;

        private bool _isFiring;
        private float _lastFireTime;

        protected override void Update()
        {
            base.Update();

            if (!_isFiring) return;
            
            var currentTargetPosition = currentAttackable.GetPosition();
            var firePointPosition = firePoint.position;
            currentTargetPosition.y = firePointPosition.y;

            transform.rotation = Quaternion.LookRotation(currentTargetPosition - firePointPosition);

            if (Time.time - _lastFireTime < fireRate) return;
            
            Fire();
            
            _lastFireTime = Time.time;
        }

        private void Fire()
        {
            var bullet = Instantiate(bulletPrefab, firePoint.position,
                Quaternion.LookRotation(firePoint.transform.forward));

            bullet.damage = damage;
            
            if (bullet.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = rb.transform.TransformDirection(bulletSpeed);
            }
        }

        protected override void OnStateChange(State newState)
        {
            base.OnStateChange(newState);

            _isFiring = newState == State.Attacking;
        }
    }
}