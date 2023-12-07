using System.Collections;
using DG.Tweening;
using MiniTowerDefence.SaveLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniTowerDefence
{
    public class WaveManager : Singleton<WaveManager>
    {
        [SerializeField]
        private Enemy meleeEnemyPrefab;

        [SerializeField]
        private RangedEnemy rangedEnemyPrefab;

        [SerializeField]
        private TMP_Text currentWaveText;

        [SerializeField]
        private Image waveProgressFill;

        private int _deadEnemyCount;
        private int _totalEnemyCount;

        private static int GetCurrentWave()
        {
            return PersistentWaveManager.Instance.GetCurrentData().currentWave;
        }

        private IEnumerator Start()
        {
            yield return StartWave();
        }

        public void OnEnemyDeath()
        {
            _deadEnemyCount++;
            RefreshProgress();

            if (_deadEnemyCount == _totalEnemyCount)
            {
                OnWaveFinish();
            }
        }

        private void OnWaveFinish()
        {
            PersistentWaveManager.Instance.IncreaseWaveCount();
            
            DataManager.SaveGame(() =>
            {
                StartCoroutine(StartWave());
            });
        }

        private IEnumerator StartWave()
        {
            yield return new WaitForSeconds(1f);
            
            currentWaveText.SetText("Wave " + (GetCurrentWave() + 1));

            var meleeEnemyCount = PersistentWaveManager.Instance.GetCurrentLevelData().meleeEnemyCount;
            var rangedEnemyCount = PersistentWaveManager.Instance.GetCurrentLevelData().rangedEnemyCount;

            _totalEnemyCount = meleeEnemyCount + rangedEnemyCount;
            _deadEnemyCount = 0;
            waveProgressFill.DOFillAmount(0f, .2f);

            for (var i = 0; i < meleeEnemyCount; i++)
            {
                Instantiate(meleeEnemyPrefab);
                yield return new WaitForSeconds(0.5f);
            }

            for (var i = 0; i < rangedEnemyCount; i++)
            {
                Instantiate(rangedEnemyPrefab);
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void RefreshProgress()
        {
            waveProgressFill.DOFillAmount((float)_deadEnemyCount / _totalEnemyCount, .2f);
        }
    }
}