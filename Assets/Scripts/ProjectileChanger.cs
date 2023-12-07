using UnityEngine;

namespace MiniTowerDefence
{
    public class ProjectileChanger : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnPoint;

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

        private void Start()
        {
            AddLauncher();
        }

        private void AddLauncher()
        {
            if (_currentLauncher != null)
            {
                Destroy(_currentLauncher);
            }

            _currentLauncher = Instantiate(ProjectileLauncherProvider.Instance.GetLauncher(LauncherType), transform);
            _currentLauncher.SetSpawnPoint(spawnPoint);
        }

        public void Launch()
        {
            if (_currentLauncher == null) return;

            _currentLauncher.Launch();
        }
    }
}