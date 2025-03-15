using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Resources.Pools;
using Newtonsoft.Json;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;

namespace ProjectF.UI.Cheats
{
    public class CheatPopupUI : PoolableBehaviourUI
    {
        public new void Initialize()
        {
            base.Initialize();
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

        private async UniTask<string> RequestCheatAsync(string command)
        {
            CheatResponse response = await NetworkManager.Instance.SendWebRequestAsync<CheatResponse>(new CheatRequest(command));
            if(response.result != ENetworkResult.Success)
                return null;

            return response.response;
        }
    }
}
