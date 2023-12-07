using UnityEngine;

namespace MiniTowerDefence
{
    public interface ISelectable
    {
        public GameObject GetSelectionDonut();
        public void Deselect();
        public void Select();
        public SelectableType GetSelectableType();
    }

    public enum SelectableType
    {
        Tower,
        Turret
    }
}