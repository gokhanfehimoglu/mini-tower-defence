using UnityEngine;
using UnityEngine.Events;

namespace MiniTowerDefence
{
    public class TimerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float duration;

        [SerializeField]
        private UnityEvent onTimerEnd = new();

        private float _timer;
        private bool _ticking;

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer > 0) return;

            _timer = 0;
            TimerEnd();
            ResetTimer();
        }

        public void ResetTimer()
        {
            _timer = duration;
        }

        private void TimerEnd()
        {
            onTimerEnd?.Invoke();
        }
    }
}