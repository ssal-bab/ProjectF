using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Farms;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using ProjectF.UI.Adventures;
using ProjectF.UI.Cheats;
using ProjectF.UI.Farms;
using UnityEngine;

namespace ProjectF.Tests
{
    public class TFarmerSpawner : MonoBehaviour
    {
        [SerializeField] AddressableAsset<Farmer> farmerPrefab = null;

        // private void Awake()
        // {
        //     farmerPrefab.Initialize();
        // }

        // private void Start()
        // {
        //     Farmer farmer = PoolManager.Spawn<Farmer>(farmerPrefab.Key);
        //     farmer?.Initialize(0);
        // }

        public async void OpenStoragePopup()
        {
            await ResourceManager.LoadResourceAsync("StoragePopupUI");
            StoragePopupUI storagePopupUI = PoolManager.Spawn<StoragePopupUI>("StoragePopupUI", GameDefine.MainPopupFrame);
            storagePopupUI.StretchRect();
            storagePopupUI.Initialize();
        }

        public async void OpenNestPopup()
        {
            await ResourceManager.LoadResourceAsync("NestPopupUI");
            NestPopupUI nestPopupUI = PoolManager.Spawn<NestPopupUI>("NestPopupUI", GameDefine.MainPopupFrame);
            nestPopupUI.StretchRect();
            nestPopupUI.Initialize();
        }

        public async void OpenSeedPocketPopup()
        {
            await ResourceManager.LoadResourceAsync("SeedPocketPopupUI");
            SeedPocketPopupUI seedPocketPopupUI = PoolManager.Spawn<SeedPocketPopupUI>("SeedPocketPopupUI", GameDefine.MainPopupFrame);
            seedPocketPopupUI.StretchRect();
            seedPocketPopupUI.Initialize();
        }

        public async void OpenCheatPopup()
        {
            await ResourceManager.LoadResourceAsync("CheatPopupUI");
            CheatPopupUI cheatPopupUI = PoolManager.Spawn<CheatPopupUI>("CheatPopupUI", GameDefine.TopPopupFrame);
            cheatPopupUI.StretchRect();
            cheatPopupUI.Initialize();
        }

        public async void OpenAdventurePopup()
        {
            await ResourceManager.LoadResourceAsync("AdventurePopupUI");
            AdventurePopupUI adventurePopupUI = PoolManager.Spawn<AdventurePopupUI>("AdventurePopupUI", GameDefine.MainPopupFrame);
            adventurePopupUI.StretchRect();
            adventurePopupUI.Initialize();
        }

        public async void OpenFieldGroupUpgradePopup(int fieldGroupID)
        {
            await ResourceManager.LoadResourceAsync("FieldGroupUpgradePopupUI");
            FieldGroupUpgradePopupUI fieldGroupUpgradePopupUI = PoolManager.Spawn<FieldGroupUpgradePopupUI>("FieldGroupUpgradePopupUI", GameDefine.ContentPopupFrame);
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

                FieldGroupData fieldGroupData = mainUser.fieldGroupData.fieldGroupDatas[response.upgradedFieldGroupID];
                fieldGroupData.level = response.currentLevel;
                fieldGroupData.OnLevelChangedEvent?.Invoke(fieldGroupData.level);

                if(ui != null)
                    ui.OnTouchCloseButton();
            }
        }
    }
}
