using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Farms;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using ProjectF.UI.Cheats;
using ProjectF.UI.Farms;
using UnityEngine;

namespace ProjectF.Tests
{
    public class TFarmerSpawner : MonoBehaviour
    {
        [SerializeField] AddressableAsset<Farmer> farmerPrefab = null;

        private void Awake()
        {
            farmerPrefab.Initialize();
        }

        private void Start()
        {
            Farmer farmer = PoolManager.Spawn<Farmer>(farmerPrefab.Key);
            farmer?.InitializeAsync(0);
        }

        public void OpenStoragePopup()
        {
            StoragePopupUI storagePopupUI = PoolManager.Spawn<StoragePopupUI>("StoragePopupUI", GameDefine.MainPopupFrame);
            storagePopupUI.StretchRect();
            storagePopupUI.Initialize();
        }

        public void OpenNestPopup()
        {
            NestPopupUI nestPopupUI = PoolManager.Spawn<NestPopupUI>("NestPopupUI", GameDefine.MainPopupFrame);
            nestPopupUI.StretchRect();
            nestPopupUI.Initialize();
        }

        public void OpenCheatPopup()
        {
            CheatPopupUI cheatPopupUI = PoolManager.Spawn<CheatPopupUI>("CheatPopupUI", GameDefine.TopPopupFrame);
            cheatPopupUI.StretchRect();
            cheatPopupUI.Initialize();
        }

        public void OpenFieldGroupUpgradePopup(int fieldGroupID)
        {
            FieldGroupUpgradePopupUI fieldGroupUpgradePopupUI = PoolManager.Spawn<FieldGroupUpgradePopupUI>("FieldGroupUpgradePopupUI", GameDefine.TopPopupFrame);
            fieldGroupUpgradePopupUI.StretchRect();
            fieldGroupUpgradePopupUI.Initialize(UpgradeFieldGroup, fieldGroupID);

            async void UpgradeFieldGroup(FieldGroupUpgradePopupUI ui)
            {
                FieldGroupUpgradeResponse response = await NetworkManager.Instance.SendWebRequestAsync<FieldGroupUpgradeResponse>(new FieldGroupUpgradeRequest(fieldGroupID));
                if (response.result != ENetworkResult.Success)
                    return;

                UserData mainUser = GameInstance.MainUser;
                mainUser.monetaData.gold -= response.usedGold;
                mainUser.storageData.materialStorage[response.usedCostItemID] -= response.usedCostItemCount;
                mainUser.fieldGroupData.fieldGroupDatas[response.upgradedFieldGroupID].level = response.currentLevel;

                if(ui != null)
                    ui.OnTouchCloseButton();
            }
        }
    }
}
