using UnityEngine;

namespace MiniTowerDefence
{
    public class AttackAreaDataBridge : MonoBehaviour
    {
        [SerializeField]
        private Soldier soldier;

        private void OnTriggerEnter(Collider other)
        {
            soldier.OnAttackAreaEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            soldier.OnAttackAreaExit(other);
        }
    }
}