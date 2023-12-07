using UnityEngine;

namespace MiniTowerDefence
{
    public abstract class Unit : MonoBehaviour, ISelectable
    {
        public abstract GameObject GetSelectionDonut();

        public void Deselect()
        {
            GetSelectionDonut().SetActive(false);
        }

        public void Select()
        {
            GetSelectionDonut().SetActive(true);
        }

        public abstract SelectableType GetSelectableType();
    }
}