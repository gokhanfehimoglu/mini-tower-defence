using UnityEngine;

namespace MiniTowerDefence
{
    public class SelectableDataBridge : MonoBehaviour
    {
        [SerializeField]
        private Unit unit;

        public ISelectable GetSelectable()
        {
            return unit;
        }
    }
}