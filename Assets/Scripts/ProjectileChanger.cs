using UnityEngine;

namespace MiniTowerDefence
{
    public class ProjectileChanger : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnPoint;

        [SerializeField]
        private Turret turret;

        private ProjectileLauncher _currentLauncher;
        private LauncherType _launcherType;

        public LauncherType LauncherType
        {
            get => _launcherType;
            set
            {
                _launcherType = value;
                AddLauncher();
            }
        }

        public void AddLauncher()
        {
            if (_currentLauncher != null)
            {
                Destroy(_currentLauncher.gameObject);
            }

            _currentLauncher = Instantiate(ProjectileLauncherProvider.Instance.GetLauncher(LauncherType), transform);
            _currentLauncher.SetSpawnPoint(spawnPoint);
            _currentLauncher.SetDamage(turret.GetDamage());
        }

        public void Launch()
        {
            if (_currentLauncher == null) return;

            _currentLauncher.Launch();
        }
    }
}