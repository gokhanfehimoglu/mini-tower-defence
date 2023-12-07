using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MiniTowerDefence
{
    public class SelectionManager : Singleton<SelectionManager>
    {
        [SerializeField]
        private LayerMask selectionLayer;

        [SerializeField]
        private GameObject towerSelectUi;

        [SerializeField]
        private GameObject turretSelectUi;

        [SerializeField]
        private CanvasGroup bottomShadow;

        private GameObject _currentActiveUi;
        private ISelectable _currentSelectable;

        public void OnClick(Vector3 mousePos)
        {
            var ray = CameraManager.Instance.mainCam.ScreenPointToRay(mousePos);
            if (!Physics.Raycast(ray, out var hit, 1000f, selectionLayer.value))
            {
                _currentSelectable?.Deselect();

                CloseUiPanel(_currentActiveUi);

                _currentSelectable = null;

                return;
            }

            if (!hit.transform.TryGetComponent(typeof(ISelectable), out var selectableComponent)) return;

            var selectable = (ISelectable)selectableComponent;

            if (_currentSelectable == selectable) return;

            _currentSelectable?.Deselect();
            CloseUiPanel(_currentActiveUi);

            _currentSelectable = selectable;
            _currentSelectable.Select();

            OpenUiPanel(GetUi(selectable.GetSelectableType()));
        }

        private void CloseUiPanel(GameObject panel)
        {
            if (panel == null) return;

            panel.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { panel.SetActive(false); });

            bottomShadow.DOFade(0f, .2f);

            _currentActiveUi = null;
        }

        private void OpenUiPanel(GameObject panel)
        {
            if (panel == null) return;

            panel.SetActive(true);
            panel.transform.DOScale(Vector3.one, 0.2f);
            
            bottomShadow.DOFade(1f, .2f);

            _currentActiveUi = panel;
        }

        private GameObject GetUi(SelectableType type)
        {
            return type switch
            {
                SelectableType.Tower => towerSelectUi,
                SelectableType.Turret => turretSelectUi,
                _ => null
            };
        }
    }
}