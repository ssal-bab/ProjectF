using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.DataTables;
using H00N.Resources;
using H00N.Resources.Pools;
using Newtonsoft.Json;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using ProjectF.UI.Farms;
using UnityEngine;

namespace ProjectF.UI.Cheats
{
    public class CheatPopupUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<CheatInputPopupUI> cheatInputPopupUIPrefab = null;

        public new async void Initialize()
        {
            base.Initialize();
            await cheatInputPopupUIPrefab.InitializeAsync();
        }
        
        public void OnTouchCloseButton()
        {
            PoolManager.Despawn(this);
        }

        public async void OnTouchAddEgg()
        {
            string response = await RequestCheatAsync("AddEgg");
            if(string.IsNullOrEmpty(response))
                return;

            List<EggHatchingData> hatchingEggList = JsonConvert.DeserializeObject<List<EggHatchingData>>(response);
            if(hatchingEggList == null)
                return;

            GameInstance.MainUser.nestData.hatchingEggList = hatchingEggList;
        }

        public void OnTouchModifyGoldButton()
        {
            PassValueCheat("ModifyGold", "돈 변경", response => {
                if(int.TryParse(response, out int modifiedValue) == false)
                    return;

                GameInstance.MainUser.monetaData.gold += modifiedValue;
            });
        }

        public void OnTouchModifySeed()
        {
            PassValueCheat("ModifySeed", "씨앗 변경", (response1, response2) => {
                if(int.TryParse(response1, out int id) == false)
                    return;

                if(int.TryParse(response2, out int count) == false)
                    return;

                GameInstance.MainUser.seedPocketData.seedStorage[id] += count;
            });
        }

        public async void OnTouchOpenFieldGroupUpgradePopupUI()
        {
            await ResourceManager.LoadResourceAsync("FieldGroupUpgradePopupUI");
            FieldGroupUpgradePopupUI fieldGroupUpgradePopupUI = PoolManager.Spawn<FieldGroupUpgradePopupUI>("FieldGroupUpgradePopupUI", GameDefine.ContentPopupFrame);
            fieldGroupUpgradePopupUI.StretchRect();
            fieldGroupUpgradePopupUI.Initialize(UpgradeFieldGroup, 0);

            async void UpgradeFieldGroup(FieldGroupUpgradePopupUI ui)
            {
                FieldGroupUpgradeResponse response = await NetworkManager.Instance.SendWebRequestAsync<FieldGroupUpgradeResponse>(new FieldGroupUpgradeRequest(0));
                if (response.result != ENetworkResult.Success)
                    return;

                UserData mainUser = GameInstance.MainUser;

                FieldGroupLevelTableRow tableRow = DataTableManager.GetTable<FieldGroupLevelTable>().GetRowByLevel(response.currentLevel - 1);
                mainUser.monetaData.gold -= tableRow.gold;
                new ApplyUpgradeCost<FieldGroupUpgradeCostTableRow>(mainUser.storageData, DataTableManager.GetTable<FieldGroupUpgradeCostTable>().GetRowListByLevel(response.currentLevel - 1));

                FieldGroupData fieldGroupData = mainUser.fieldGroupData.fieldGroupDatas[response.upgradedFieldGroupID];
                fieldGroupData.level = response.currentLevel;
                fieldGroupData.OnLevelChangedEvent?.Invoke(fieldGroupData.level);

                if(ui != null)
                    ui.OnTouchCloseButton();
            }
        }

        private void PassValueCheat(string command, string description, Action<string> callback)
        {
            CheatInputPopupUI inputPopupUI = PoolManager.Spawn(cheatInputPopupUIPrefab, GameDefine.TopPopupFrame);
            inputPopupUI.StretchRect();
            inputPopupUI.Initialize(description, async input => {
                if(int.TryParse(input, out int value) == false)
                    return;

                string response = await RequestCheatAsync(command, value.ToString());
                if (string.IsNullOrEmpty(response))
                {
                    Debug.Log($"Response in null. value : {value}");
                    return;
                }

                callback?.Invoke(response);
            });
        }

        private void PassValueCheat(string command, string description, Action<string, string> callback)
        {
            CheatInputPopupUI inputPopupUI = PoolManager.Spawn(cheatInputPopupUIPrefab, GameDefine.TopPopupFrame);
            inputPopupUI.StretchRect();
            inputPopupUI.Initialize(description, async (input1, input2) => {
                if(int.TryParse(input1, out int value1) == false)
                    return;

                if(int.TryParse(input2, out int value2) == false)
                    return;

                string response = await RequestCheatAsync(command, value1.ToString(), value2.ToString());
                if (string.IsNullOrEmpty(response))
                {
                    Debug.Log($"Response in null. value1 : {value1}, value2 : {value2}");
                    return;
                }

                string[] responses = JsonConvert.DeserializeObject<string[]>(response);
                callback?.Invoke(responses[0], responses[1]);
            });
        }

        private async UniTask<string> RequestCheatAsync(string command, params string[] option)
        {
            CheatResponse response = await NetworkManager.Instance.SendWebRequestAsync<CheatResponse>(new CheatRequest(command, option));
            if(response.result != ENetworkResult.Success)
                return null;

            return response.response;
        }
    }
}
