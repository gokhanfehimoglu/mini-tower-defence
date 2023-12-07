using System;
using MiniTowerDefence.SaveLogic;
using MiniTowerDefence.SaveLogic.Model;
using TMPro;
using UnityEngine;

namespace MiniTowerDefence
{
    public class CurrencyManager : Singleton<CurrencyManager>, ISaveable
    {
        public CurrencyData data;

        public GuidComponent guidComponent;

        [SerializeField]
        private TMP_Text currencyText;

        private void Start()
        {
            DataManager.Entities.Add(this);
            GetLoadedData();
            RefreshUi();
        }

        private void GetLoadedData()
        {
            if (DataManager.Instance == null) return;
            if (DataManager.Instance.GetEntityData(guidComponent.GetGuid()) is not CurrencySaveData entityData) return;
            data = entityData.data;
        }

        [Serializable]
        public class CurrencySaveData : DataManager.SaveData
        {
            public CurrencyData data;
        }

        public DataManager.SaveData GetSaveData()
        {
            return new CurrencySaveData
            {
                guid = guidComponent.GetGuid().ToString(),
                data = data
            };
        }

        public void OnEnemyDeath()
        {
            data.money += 10;
            RefreshUi();
        }

        private void RefreshUi()
        {
            currencyText.SetText(data.money + "");
        }

        public int GetCurrentMoney()
        {
            return data.money;
        }

        public bool Spend(int amount)
        {
            if (amount > data.money) return false;
            data.money -= amount;
            RefreshUi();
            return true;
        }
    }
}