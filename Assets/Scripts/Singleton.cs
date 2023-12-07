using System;
using UnityEngine;

namespace MiniTowerDefence
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var objects = FindObjectsOfType(typeof(T)) as T[];
                if (objects?.Length > 0)
                    _instance = objects[0];

                if (objects?.Length > 1)
                {
                    throw new Exception("More than 1 " + typeof(T).Name + " instance");
                }

                if (_instance != null) return _instance;

                Debug.LogWarning("No instance of " + typeof(T).Name + " found in the scene. Creating new");

                var obj = new GameObject
                {
                    name = $"_{typeof(T).Name}"
                };

                _instance = obj.AddComponent<T>();
                return _instance;
            }
        }
    }
}