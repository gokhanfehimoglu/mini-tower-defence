using UnityEngine;

namespace MiniTowerDefence
{
    public abstract class ProjectileLauncher : MonoBehaviour
    {
        [SerializeField]
        protected Bullet projectilePrefab;
        
        [SerializeField]
        protected Vector3 speed;

        [SerializeField]
        protected float damage;

        protected Transform spawnPoint;

        public abstract void Launch();

        public void SetSpawnPoint(Transform t)
        {
            spawnPoint = t;
        }

        public void SetDamage(float value)
        {
            damage = value;
        }
    }
}