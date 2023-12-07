using UnityEngine;

namespace MiniTowerDefence.StateMachine
{
    public abstract class Condition : MonoBehaviour
    {
        public abstract bool IsMet();
    }
}