using System.Collections.Generic;
using H00N.Resources;
using H00N.Resources.Pools;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventurePopupUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<AdventureAreaPopupUI> areaPopupUIPrefab = null;
        [SerializeField] List<AdventureAreaElementUI> areaElementUIList = null;

        public new async void Initialize()
        {
            base.Initialize();
            await areaPopupUIPrefab.InitializeAsync();

            foreach(AdventureAreaElementUI elementUI in areaElementUIList)
                elementUI.Initialize(OpenAdventureAreaPopupUI);
        }

        private void OpenAdventureAreaPopupUI(int areaID)
        {
            AdventureAreaPopupUI areaPopupUI = PoolManager.Spawn(areaPopupUIPrefab);
            areaPopupUI.Initialize(areaID);
        }
    }
}
