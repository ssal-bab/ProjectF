using System;
using System.Collections;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Drawing.Charts;
using H00N.Extensions;
using H00N.Resources.Pools;
using ProjectF.Farms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public enum OrderType
    {
        Ascending = 0,
        Descending = 180
    }

    public enum ClassificationType
    {
        Acquisition,
        Rarity,
        Name,
        Level
    }

    public class FarmerListPopupUI : PoolableBehaviourUI
    {
        // 외부 클래스로 빼야 할 항목 : 일꾼 목록 Element, 일꾼 Info Popup
        // 해당 클래스 내에서 구현해야 할 항목 : 

        [SerializeField] private RectTransform _orderButtonVisualTrm;
        [SerializeField] private Image _salesModeFilter;
        [SerializeField] private GameObject _salesButton;
        [SerializeField] private TextMeshProUGUI _salesAllowanceText;
        [SerializeField] private FarmerListUI _farmerListUI;

        public new void Initialize()
        {
            base.Initialize();
        }

        public void ActiveSalesFarmerFilter(bool isActive)
        {
            _salesModeFilter.raycastTarget = isActive;
            _salesButton.SetActive(isActive);
        }

        public void SalesFarmer()
        {

        }

        // 분류 항목 기준 변경
        public void ChangeElementClassification(string classification)
        {
            _farmerListUI.ChangeClassification((ClassificationType)Enum.Parse(typeof(ClassificationType), classification));
        }

        // 오름차순 내림차순 변경
        public void ChangeOrder()
        {
            var currentOrderType = _farmerListUI.ChangeOrder();
            _orderButtonVisualTrm.rotation = Quaternion.Euler(0, 0, (int)currentOrderType);
        }

        protected override void Release()
        {
            base.Release();
        }
    }
}
