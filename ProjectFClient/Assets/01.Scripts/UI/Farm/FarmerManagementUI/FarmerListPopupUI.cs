using System.Collections.Generic;
using H00N.Resources.Pools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProjectF.Networks;
using ProjectFServer.Networks.Packets;
using System.Linq;

namespace ProjectF.UI.Farms
{
    public struct FarmerSalesInfo
    {
        public string farmerUUID;
        public int farmerLevel;

        public FarmerSalesInfo(string farmerUUID, int farmerLevel)
        {
            this.farmerUUID = farmerUUID;
            this.farmerLevel = farmerLevel;
        }
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

        private Dictionary<string, FarmerSalesInfo> farmerSalesDic = new();

        public new void Initialize()
        {
            base.Initialize();
            _farmerListUI.Initialize(RegisterSalesFarmer, UnRegisterSalesFarmer);

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
            return farmerSalesDic.ContainsKey(uuid);
        }

        public void RegisterSalesFarmer(string farmerUUID, int farmerLevel)
        {
            if (IsRegisterSlaesFarmer(farmerUUID)) return;

            var info = new FarmerSalesInfo(farmerUUID, farmerLevel);
            farmerSalesDic.Add(farmerUUID, info);

        }

        public void UnRegisterSalesFarmer(string farmerUUID)
        {
            if (!IsRegisterSlaesFarmer(farmerUUID)) return;

            farmerSalesDic.Remove(farmerUUID);
        }

        public async void FarmerSaleAsync()
        {
            var farmerDataList = farmerSalesDic.Keys.Where(farmerSalesDic.ContainsKey).Select(k => farmerSalesDic[k]);

            foreach (var info in farmerDataList)
            {
                if (!GameInstance.MainUser.farmerData.farmerList.ContainsKey(info.farmerUUID))
                {
                    Debug.LogError("일꾼을 찾을 수 없습니다.");
                    return;
                }
            }

            var req = new FarmerSalesRequest(farmerSalesDic.Keys.ToArray());
            var response = await NetworkManager.Instance.SendWebRequestAsync<FarmerSalesResponse>(req);

            if (response.result != ENetworkResult.Success)
            {
                Debug.LogError("Critical Error!");
                return;
            }

            var mainUser = GameInstance.MainUser;

            foreach(var info in farmerDataList)
            {
                var farmerData = mainUser.farmerData.farmerList[info.farmerUUID];

                mainUser.monetaData.gold += new CalculateFarmerSalesAllowance(farmerData.rarity, farmerData.level).value;
                mainUser.farmerData.farmerList.Remove(info.farmerUUID);
            }
        }

        // 분류 항목 기준 변경
        public void OnChangeElementClassification(int classificationIdx)
        {
            _farmerListUI.ChangeClassification((EFarmerClassificationType)classificationIdx);
        }

        // 오름차순 내림차순 변경
        public void OnChangeOrder()
        {
            var currentOrderType = _farmerListUI.ChangeOrder();
            _orderButtonVisualTrm.rotation = Quaternion.Euler(0, 0, (int)currentOrderType * 180);
            _farmerListUI.ChangeOrder();
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
