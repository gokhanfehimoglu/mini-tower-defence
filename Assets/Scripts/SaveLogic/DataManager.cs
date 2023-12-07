using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniTowerDefence.SaveLogic
{
    public class DataManager : Singleton<DataManager>
    {

        private const string SaveFileName = "/save.bin";
        private static readonly JsonSerializerSettings Settings = new() { TypeNameHandling = TypeNameHandling.Auto };

        public static readonly List<ISaveable> Entities = new();
        private Data loadedData;

        private IEnumerator Start()
        {
            DontDestroyOnLoad(gameObject);
            yield return StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            if (File.Exists(Application.persistentDataPath + SaveFileName))
            {
                yield return LoadGame(data =>
                {
                    loadedData = data;

                    var waveData = GetWaveData();

                    PersistentWaveManager.Instance.SetData(waveData.data);

                    var currentWave = waveData.data.currentWave;

                    SceneManager.LoadScene("MainScene");
                });
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        public static void LoadSceneAndSaveData(string sceneName)
        {
            ClearEntities();
            SaveGame(() => { SceneManager.LoadScene(sceneName); });
        }

        private PersistentWaveManager.WaveSaveData GetWaveData()
        {
            return GetEntityData(PersistentWaveManager.Instance.guidComponent.GetGuid()) as
                PersistentWaveManager.WaveSaveData;
        }

        public static async void SaveGame(Action cb = null)
        {
            var data = new Data
            {
                entityData = Entities.Select(e => e.GetSaveData()).ToList()
            };

            await SaveActual(data, SaveFileName);
            cb?.Invoke();
        }

        private static async Task LoadGame(Action<Data> cb)
        {
            var data = await LoadActual(SaveFileName);
            cb.Invoke(data);
        }

        private static async Task<Data> LoadActual(string fileName)
        {
            fileName = Application.persistentDataPath + fileName;

            var encrypted = await FileAsync.ReadAllBytes(fileName);

            var jsonData = EncrypterAes.DecryptStringFromBytes_Aes(encrypted);

            return JsonConvert.DeserializeObject<Data>(jsonData, Settings);
        }

        private static async Task SaveActual(Data saveData, string fileName)
        {
            fileName = Application.persistentDataPath + fileName;
            var jsonData = JsonConvert.SerializeObject(saveData, Settings);
            var encrypted = EncrypterAes.EncryptStringToBytes_Aes(jsonData);

            await FileAsync.WriteAllBytes(fileName, encrypted);
        }

        public void RestartGame()
        {
            StartCoroutine(StartGame());
        }

        public void EndGame()
        {
            File.Delete(Application.persistentDataPath + SaveFileName);
            ClearEntities();
            PersistentWaveManager.Instance.RestartGame();
            RestartGame();
        }

        [Serializable]
        public class SaveData
        {
            public string guid;
        }

        [Serializable]
        public class Data
        {
            public List<SaveData> entityData;
        }

        public SaveData GetEntityData(Guid guid)
        {
            return loadedData?.entityData.SingleOrDefault(e => e.guid == guid.ToString());
        }

        private static void ClearEntities()
        {
            Entities.Clear();
            Entities.Add(PersistentWaveManager.Instance);
        }
    }
}