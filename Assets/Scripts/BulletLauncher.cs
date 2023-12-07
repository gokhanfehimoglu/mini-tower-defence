using UnityEngine;

namespace MiniTowerDefence
{
    public class BulletLauncher : ProjectileLauncher
    {
        [SerializeField]
        private Vector3 speed;
        
        public override void Launch()
        {
            var bullet = Instantiate(projectilePrefab, spawnPoint.position,
                Quaternion.LookRotation(spawnPoint.transform.up));

            if (bullet.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = rb.transform.TransformDirection(speed);
            }
        }
    }
}