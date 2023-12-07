using UnityEngine;

namespace MiniTowerDefence
{
    public abstract class ProjectileLauncher : MonoBehaviour
    {
        [SerializeField]
        protected GameObject projectilePrefab;

        protected Transform spawnPoint;

        public abstract void Launch();

        public void SetSpawnPoint(Transform t)
        {
            spawnPoint = t;
        }
    }
}