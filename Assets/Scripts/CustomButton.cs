using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniTowerDefence
{
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Image colorBox;

        [SerializeField]
        private Image shadowBox;

        private Tween _tween;

        public void OnPointerDown(PointerEventData eventData)
        {
            _tween?.Kill();
            _tween = colorBox.rectTransform.DOAnchorPosY(-7.5f, 0.2f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _tween?.Kill();
            _tween = colorBox.rectTransform.DOAnchorPosY(7.5f, 0.2f);
        }

        public void SetColor(Color color)
        {
            colorBox.color = color;
            Color.RGBToHSV(color, out var h, out var s, out var v);
            shadowBox.color = Color.HSVToRGB(h, s, v - 0.2f);
        }
    }
}