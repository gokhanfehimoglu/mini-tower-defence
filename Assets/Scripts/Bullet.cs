using UnityEngine;

namespace MiniTowerDefence
{
    public class Bullet : MonoBehaviour
    {
        public float damage;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(typeof(IAttackable), out var attackable)) return;

            (attackable as IAttackable)?.Attacked(damage);
            Destroy(gameObject);
        }
    }
}