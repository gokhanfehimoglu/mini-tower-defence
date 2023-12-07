using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniTowerDefence
{
    public class ProjectileLauncherProvider : Singleton<ProjectileLauncherProvider>
    {
        public List<LauncherTypeWithPrefab> prefabs;

        public ProjectileLauncher GetLauncher(LauncherType type)
        {
            var prefab = prefabs.FirstOrDefault(p => p.launcherType == type);
            return prefab?.projectileLauncher;
        }
    }

    [Serializable]
    public class LauncherTypeWithPrefab
    {
        public LauncherType launcherType;
        public ProjectileLauncher projectileLauncher;
    }
}