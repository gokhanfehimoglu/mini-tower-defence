using UnityEngine;
using UnityEngine.Events;

namespace MiniTowerDefence
{
    public interface IAttackable
    {
        void Attacked(float damage);
        Vector3 GetPosition();
        UnityEvent<Collider> GetOnDeathNotifier();
    }
}