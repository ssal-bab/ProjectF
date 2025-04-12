using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class NestInfoPanelUI : MonoBehaviourUI
    {
        [SerializeField] Image nestIconImage = null;
        [SerializeField] TMP_Text nameText = null;

        [SerializeField] SliderUI eggSliderUI = null;
        [SerializeField] SliderUI farmerSliderUI = null;
        
        [Space(10f)]
        [SerializeField] GameObject upgradeButtonObject = null;
        [SerializeField] GameObject upgradeCompleteButtonObject = null;
        [SerializeField] AddressableAsset<NestUpgradePopupUI> upgradePopupUIPrefab = null;

        public new async void Initialize()
        {
            base.Initialize();

            RefreshUI();
            await upgradePopupUIPrefab.InitializeAsync();
        }

        private void RefreshUI()
        {
            UserNestData nestData = GameInstance.MainUser.nestData;
            GetFacilityTableRow<NestTable, NestTableRow> getFacilityTableRow = new GetFacilityTableRow<NestTable, NestTableRow>(nestData.level);

            NestTableRow tableRow = getFacilityTableRow.currentTableRow;
            if (tableRow == null)
            {
                Debug.LogError($"[NestInfoPanelUI::RefreshUI] tableRow is null. CurrentLevel : {nestData.level}");
                return;
            }

            upgradeButtonObject.SetActive(!getFacilityTableRow.isMaxLevel);
            upgradeCompleteButtonObject.SetActive(getFacilityTableRow.isMaxLevel);

            new SetSprite(nestIconImage, ResourceUtility.GetStorageIconKey(tableRow.id));
            nameText.text = $"Lv.{tableRow.level} 둥지{tableRow.level}"; // 나중에 localizing 적용해야 함

            eggSliderUI.RefreshUI(tableRow.eggStoreLimit, nestData.hatchingEggList.Count);
            farmerSliderUI.RefreshUI(tableRow.farmerStoreLimit, GameInstance.MainUser.farmerData.farmerList.Count);
        }

        public void OnTouchUpgradeButton()
        {
            int currentLevel = GameInstance.MainUser.nestData.level;
            GetFacilityTableRow<NestTable, NestTableRow> getFacilityTableRow = new GetFacilityTableRow<NestTable, NestTableRow>(currentLevel);
            if(getFacilityTableRow.isMaxLevel)
            {
                Debug.LogError($"[NestInfoPanelUI::OnTouchUpgradeButton] Already max Level, but trying to open NestUpgradePopupUI. CurrentLevel : {currentLevel}");
                return;
            }

            NestUpgradePopupUI upgradePopupUI = PoolManager.Spawn<NestUpgradePopupUI>(upgradePopupUIPrefab, GameDefine.ContentPopupFrame);
            upgradePopupUI.StretchRect();
            upgradePopupUI.Initialize(UpgradeNest);
        }

        private async void UpgradeNest(NestUpgradePopupUI ui)
        {
            NestUpgradeResponse response = await NetworkManager.Instance.SendWebRequestAsync<NestUpgradeResponse>(new NestUpgradeRequest());
            if (response.result != ENetworkResult.Success)
                return;

            UserData mainUser = GameInstance.MainUser;
            mainUser.monetaData.gold -= response.usedGold;
            mainUser.storageData.materialStorage[response.usedCostItemID] -= response.usedCostItemCount;
            mainUser.nestData.level = response.currentLevel;
            mainUser.nestData.OnLevelChangedEvent?.Invoke(mainUser.nestData.level);

            if(ui != null)
                ui.OnTouchCloseButton();

            RefreshUI();
        }
    }
}