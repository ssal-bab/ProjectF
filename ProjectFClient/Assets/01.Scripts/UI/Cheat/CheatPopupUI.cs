using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Resources;
using H00N.Resources.Pools;
using Newtonsoft.Json;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF.UI.Cheats
{
    public class CheatPopupUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<CheatInputPopupUI> cheatInputPopupUIPrefab = null;

        public new void Initialize()
        {
            base.Initialize();
            cheatInputPopupUIPrefab.Initialize();
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

        private async UniTask<string> RequestCheatAsync(string command, params string[] option)
        {
            CheatResponse response = await NetworkManager.Instance.SendWebRequestAsync<CheatResponse>(new CheatRequest(command, option));
            if(response.result != ENetworkResult.Success)
                return null;

            return response.response;
        }
    }
}
