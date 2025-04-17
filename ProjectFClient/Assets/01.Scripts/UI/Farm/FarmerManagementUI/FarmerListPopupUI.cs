using System.Collections.Generic;
using H00N.Resources.Pools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using System.Linq;
using H00N.DataTables;
using ProjectF.DataTables;

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

        [SerializeField] private RectTransform orderButtonVisualTrm;
        [SerializeField] private Image salesModeFilter;
        [SerializeField] private GameObject salesButton;
        [SerializeField] private TextMeshProUGUI salesAllowanceText;
        [SerializeField] private TextMeshProUGUI farmerCountText;
        [SerializeField] private FarmerListUI farmerListUI;

        private Dictionary<string, FarmerSalesInfo> farmerSalesDic = new();

        public new void Initialize()
        {
            base.Initialize();
            farmerListUI.Initialize(RegisterSalesFarmer, UnRegisterSalesFarmer);

            ActiveSalesFarmerMode(false);

            var farmerTable = DataTableManager.GetTable<FarmerTable>();
            var farmerData = GameInstance.MainUser.farmerData;

            farmerCountText.text = $"{farmerData.farmerList.Count}/{farmerTable.Count()}";
        }

        public void ActiveSalesFarmerMode(bool isActive)
        {
            farmerListUI.ActiveSalesFarmerMode(isActive, this);
            salesModeFilter.raycastTarget = isActive;
            salesButton.SetActive(isActive);
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
            farmerListUI.ChangeClassification((EFarmerClassificationType)classificationIdx);
        }

        // 오름차순 내림차순 변경
        public void OnChangeOrder()
        {
            var currentOrderType = farmerListUI.ChangeOrder();
            orderButtonVisualTrm.rotation = Quaternion.Euler(0, 0, (int)currentOrderType * 180);
            farmerListUI.ChangeOrder();
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
        }

        protected override void Release()
        {
            base.Release();
            farmerListUI.Release();
            ActiveSalesFarmerMode(false);
        }
    }
}
