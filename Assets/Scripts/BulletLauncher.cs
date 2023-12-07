using UnityEngine;

namespace MiniTowerDefence
{
    public class BulletLauncher : ProjectileLauncher
    {
        public override void Launch()
        {
            var bullet = Instantiate(projectilePrefab, spawnPoint.position,
                Quaternion.LookRotation(spawnPoint.transform.up));

            bullet.damage = damage;

            if (bullet.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = rb.transform.TransformDirection(speed);
            }
        }
    }
}