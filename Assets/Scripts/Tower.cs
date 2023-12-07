using MiniTowerDefence.SaveLogic;
using UnityEngine;
using UnityEngine.Events;

namespace MiniTowerDefence
{
    public class Tower : Unit, IAttackable
    {
        [SerializeField]
        private GameObject selectionDonut;

        [SerializeField]
        private float health;
        
        [SerializeField]
        private SphereCollider bodyCollider;

        private readonly UnityEvent<Collider> _onDeathNotifier = new();

        public override GameObject GetSelectionDonut()
        {
            return selectionDonut;
        }

        public override SelectableType GetSelectableType()
        {
            return SelectableType.Tower;
        }

        public void Attacked(float receivedDamage)
        {
            health -= receivedDamage;

            if (health > 0) return;

            _onDeathNotifier?.Invoke(bodyCollider);

            Destroy(gameObject);
            DataManager.Instance.EndGame();
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public UnityEvent<Collider> GetOnDeathNotifier()
        {
            return _onDeathNotifier;
        }
    }
}