using System;
using UnityEngine;
using UnityEngine.Events;

namespace MiniTowerDefence
{
    public class TargetableBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Transform targetPoint;

        public Transform TargetPoint => targetPoint;
    }

    [Serializable]
    public class UnityTargetableEvent : UnityEvent<TargetableBehaviour>
    {
    }
}