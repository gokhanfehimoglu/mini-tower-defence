using System;
using System.Collections.Generic;
using System.Linq;
using MiniTowerDefence.SaveLogic;
using UnityEngine;

namespace MiniTowerDefence
{
    public class Turret : Unit, ISaveable
    {
        [SerializeField]
        private GameObject selectionDonut;

        [SerializeField]
        private ProjectileChanger projectileChanger;

        [SerializeField]
        private TimerBehaviour timerBehaviour;
        
        public GuidComponent guidComponent;
        
        [SerializeField]
        private TurretData data;

        [SerializeField]
        private List<TurretLevelData> levelData;

        [SerializeField]
        private int turretNumber;

        private void Start()
        {
            DataManager.Entities.Add(this);
            GetLoadedData();
            timerBehaviour.SetDuration(levelData[data.currentLevel].fireRate);
            projectileChanger.AddLauncher();
        }
        
        private void GetLoadedData()
        {
            if (DataManager.Instance == null) return;
            if (DataManager.Instance.GetEntityData(guidComponent.GetGuid()) is not TurretSaveData entityData) return;
            SetData(entityData);
        }

        private void SetData(TurretSaveData saveData)
        {
            data = saveData.data;
            levelData = saveData.levelData;
        }

        public override GameObject GetSelectionDonut()
        {
            return selectionDonut;
        }

        public override SelectableType GetSelectableType()
        {
            return SelectableType.Turret;
        }
        
        [Serializable]
        public class TurretSaveData : DataManager.SaveData
        {
            public TurretData data;
            public List<TurretLevelData> levelData;
        }

        public DataManager.SaveData GetSaveData()
        {
            return new TurretSaveData
            {
                guid = guidComponent.GetGuid().ToString(),
                data = data,
                levelData = levelData.ToList()
            };
        }

        public float GetDamage()
        {
            return levelData[data.currentLevel].damage;
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
            projectileChanger.AddLauncher();
            timerBehaviour.SetDuration(levelData[data.currentLevel].fireRate);
        }
    }
}