using System;
using System.Collections.Generic;
using System.Linq;
using MiniTowerDefence.SaveLogic;
using UnityEngine;

namespace MiniTowerDefence
{
    public class TowerManager : Singleton<TowerManager>, ISaveable
    {
        [SerializeField]
        private TowerData data;
        
        [SerializeField]
        private List<TowerLevelData> levelData;
        
        [SerializeField]
        private Turret turret2;

        [SerializeField]
        private Turret turret3;

        public GuidComponent guidComponent;

        private void Start()
        {
            DataManager.Entities.Add(this);
            GetLoadedData();

            if (data.currentLevel > 1)
            {
                turret2.gameObject.SetActive(true);
            }

            if (data.currentLevel > 2)
            {
                turret3.gameObject.SetActive(true);
            }
        }

        private void GetLoadedData()
        {
            if (DataManager.Instance == null) return;
            if (DataManager.Instance.GetEntityData(guidComponent.GetGuid()) is not TowerSaveData entityData) return;
            SetData(entityData);
        }

        private void SetData(TowerSaveData saveData)
        {
            data = saveData.data;
            levelData = saveData.levelData;
        }

        [Serializable]
        public class TowerSaveData : DataManager.SaveData
        {
            public TowerData data;
            public List<TowerLevelData> levelData;
        }

        public DataManager.SaveData GetSaveData()
        {
            return new TowerSaveData
            {
                guid = guidComponent.GetGuid().ToString(),
                data = data,
                levelData = levelData.ToList()
            };
        }
        
        public float GetPrice()
        {
            return levelData[data.currentLevel].price;
        }

        public bool IsLevelMax()
        {
            return data.currentLevel == data.maxLevel;
        }

        public float GetLevel()
        {
            return data.currentLevel;
        }

        public void LevelUp()
        {
            if (IsLevelMax()) return;
            data.currentLevel++;
            
            if (data.currentLevel > 1)
            {
                turret2.gameObject.SetActive(true);
                turret2.LevelUp();
            }

            if (data.currentLevel > 2)
            {
                turret3.gameObject.SetActive(true);
                turret3.LevelUp();
            }
        }

        public void GameRestarted()
        {
            turret2.gameObject.SetActive(false);
            turret3.gameObject.SetActive(false);
        }
    }
}