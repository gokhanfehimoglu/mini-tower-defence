using UnityEngine;

namespace MiniTowerDefence
{
    public class Tower : Unit
    {
        [SerializeField]
        private GameObject selectionDonut;

        public override GameObject GetSelectionDonut()
        {
            return selectionDonut;
        }

        public override SelectableType GetSelectableType()
        {
            return SelectableType.Tower;
        }
    }
}