using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            DOTween.Init();
        }
    }
}