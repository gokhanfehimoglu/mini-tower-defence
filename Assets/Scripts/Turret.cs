using UnityEngine;

namespace MiniTowerDefence
{
    public class Turret : Unit
    {
        [SerializeField]
        private GameObject selectionDonut;

        public override GameObject GetSelectionDonut()
        {
            return selectionDonut;
        }

        public override SelectableType GetSelectableType()
        {
            return SelectableType.Turret;
        }
    }
}