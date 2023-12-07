using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniTowerDefence
{
    public class ClickDetector : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SelectionManager.Instance.OnClick(Input.mousePosition);
        }
    }
}