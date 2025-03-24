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
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectFServer.Networks.Packets;
using H00N.DataTables;
using ProjectF.Datas;
using System.Linq;

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

        private Dictionary<string, (string, int)> _farmerSalesDic = new();

        public new void Initialize()
        {
            base.Initialize();
            _farmerListUI.Initialize();

            ActiveSalesFarmerMode(false);
        }

        public void ActiveSalesFarmerMode(bool isActive)
        {
            _farmerListUI.ActiveSalesFarmerMode(isActive, this);
            _salesModeFilter.raycastTarget = isActive;
            _salesButton.SetActive(isActive);
        }

        private bool IsRegisterSlaesFarmer(string uuid)
        {
            return _farmerSalesDic.ContainsKey(uuid);
        }

        public void RegisterSalesFarmer(string farmerUUID, int farmerLevel)
        {
            if (IsRegisterSlaesFarmer(farmerUUID)) return;

            _farmerSalesDic.Add(farmerUUID, (farmerUUID, farmerLevel));

        }

        public void UnRegisterSalesFarmer(string farmerUUID)
        {
            if (!IsRegisterSlaesFarmer(farmerUUID)) return;

            _farmerSalesDic.Remove(farmerUUID);
        }

        public async void FarmerSaleAsync()
        {
            int salesAllowance = 0;

            foreach (var farmerInfo in _farmerSalesDic)
            {
                salesAllowance += GetFarmerSalesAllowance(farmerInfo.Value.Item1, farmerInfo.Value.Item2);
            }

            var req = new FarmerSalesRequest(_farmerSalesDic.Keys.ToArray(), salesAllowance);
            var response = await NetworkManager.Instance.SendWebRequestAsync<FarmerSalesResponse>(req);

            if (response.result != ENetworkResult.Success)
            {
                Debug.LogError(response.result);
            }
            else
            {
                Debug.Log("판매 성공");
            }
        }

        private int GetFarmerSalesAllowance(string farmerUUID, int farmerLevel)
        {
            ERarity rarity = GameInstance.MainUser.farmerData.farmerList[farmerUUID].rarity;

            FarmerConfigTable farmerConfigTable = DataTableManager.GetTable<FarmerConfigTable>();
            float farmingMultiplier = farmerLevel * farmerConfigTable.LevelSalesMultiplierValue();
            float gradeMultiplier = (int)rarity * farmerConfigTable.GradeSalesMultiplierValue();

            return Mathf.FloorToInt(farmingMultiplier + gradeMultiplier);
        }

        // 분류 항목 기준 변경
        public void OnChangeElementClassification(int classificationIdx)
        {
            _farmerListUI.ChangeClassification((ClassificationType)classificationIdx);
        }

        // 오름차순 내림차순 변경
        public void OnChangeOrder()
        {
            var currentOrderType = _farmerListUI.ChangeOrder();
            _orderButtonVisualTrm.rotation = Quaternion.Euler(0, 0, (int)currentOrderType);
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.DespawnAsync(this);
        }

        protected override void Release()
        {
            base.Release();
            _farmerListUI.Release();
            ActiveSalesFarmerMode(false);
        }
    }
}
