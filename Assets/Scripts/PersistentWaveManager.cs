using System;
using System.Collections.Generic;
using MiniTowerDefence.SaveLogic;
using MiniTowerDefence.SaveLogic.Model;
using UnityEngine;

namespace MiniTowerDefence
{
    public class PersistentWaveManager : Singleton<PersistentWaveManager>, ISaveable
    {
        public GuidComponent guidComponent;
        public bool worldChanged;
        
        [SerializeField]
        private WaveData data;
    
        [SerializeField]
        private List<WaveLevelData> levelData;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            DataManager.Entities.Add(this);
        }
    
        public void SetData(WaveData loadedData)
        {
            data = loadedData;
        }
        
        [Serializable]
        public class WaveSaveData : DataManager.SaveData
        {
            public WaveData data;
        }
    
        public DataManager.SaveData GetSaveData()
        {
            return new WaveSaveData
            {
                guid = guidComponent.GetGuid().ToString(),
                data = data
            };
        }
    
        public WaveLevelData GetCurrentLevelData()
        {
            return levelData[data.currentWave];
        }
    
        public WaveData GetCurrentData()
        {
            return data;
        }
    
        public void IncreaseWaveCount()
        {
            if (levelData.Count == data.currentWave + 1) return;
            data.currentWave++;
        }
    
        public void RestartGame()
        {
            data.currentWave = 0;
            worldChanged = false;
            TowerManager.Instance.GameRestarted();
        }
    }
}