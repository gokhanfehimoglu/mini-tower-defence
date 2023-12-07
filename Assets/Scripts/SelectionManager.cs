using DefaultNamespace;
using DG.Tweening;
using TMPro;
using UnityEngine;

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

        [SerializeField]
        private TMP_Text turretUpgradePriceText;

        [SerializeField]
        private TMP_Text turretLevelText;

        [SerializeField]
        private CustomButton turretUpgradeButton;
        
        [SerializeField]
        private TMP_Text towerUpgradePriceText;

        [SerializeField]
        private TMP_Text towerLevelText;

        [SerializeField]
        private CustomButton towerUpgradeButton;

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

            if (!hit.transform.TryGetComponent(typeof(SelectableDataBridge), out var selectableDataBridge)) return;

            var selectable = ((SelectableDataBridge)selectableDataBridge).GetSelectable();

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
            switch (type)
            {
                case SelectableType.Turret:
                    var turret = (Turret)_currentSelectable;

                    if (turret.IsLevelMax())
                    {
                        turretUpgradePriceText.SetText("MAXED");
                        turretUpgradeButton.SetColor(Color.gray);
                        turretUpgradeButton.SetDisabled();
                    }
                    else
                    {
                        turretLevelText.SetText("UPGRADE TO LEVEL " + (turret.GetLevel() + 1));
                        turretUpgradePriceText.SetText(turret.GetPrice() + "");
                        ColorUtility.TryParseHtmlString("#0094FF", out var newColor);
                        turretUpgradeButton.SetColor(newColor);
                        turretUpgradeButton.SetEnabled();
                    }

                    return turretSelectUi;
                case SelectableType.Tower:
                    if (TowerManager.Instance.IsLevelMax())
                    {
                        towerUpgradePriceText.SetText("MAXED");
                        towerUpgradeButton.SetColor(Color.gray);
                        towerUpgradeButton.SetDisabled();
                    }
                    else
                    {
                        towerLevelText.SetText("ADD NEW TURRET");
                        towerUpgradePriceText.SetText(TowerManager.Instance.GetPrice() + "");
                        ColorUtility.TryParseHtmlString("#0094FF", out var newColor);
                        turretUpgradeButton.SetColor(newColor);
                        towerUpgradeButton.SetEnabled();
                    }
                    
                    return towerSelectUi;
            }

            return null;
        }

        public void UpgradeTurretClicked()
        {
            var turret = (Turret)_currentSelectable;
            var price = turret.GetPrice();
            var purchased = CurrencyManager.Instance.Spend((int)price);
            if (!purchased)
            {
                print("not enough money");
                return;
            }
            
            ((Turret)_currentSelectable).LevelUp();    
            OpenUiPanel(GetUi(_currentSelectable.GetSelectableType()));
        }
        
        public void AddTurretClicked()
        {
            var price = TowerManager.Instance.GetPrice();
            var purchased = CurrencyManager.Instance.Spend((int)price);
            if (!purchased)
            {
                print("not enough money");
                return;
            }
            
            TowerManager.Instance.LevelUp();    
            OpenUiPanel(GetUi(_currentSelectable.GetSelectableType()));
        }
    }
}