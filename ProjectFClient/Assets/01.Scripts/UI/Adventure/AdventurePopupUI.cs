using System.Collections.Generic;
using H00N.Resources;
using H00N.Resources.Pools;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventurePopupUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<AdventureAreaUpgradePopupUI> upgradePopupUIPrefab = null;
        [SerializeField] AddressableAsset<AdventureAreaPopupUI> areaPopupUIPrefab = null;
        
        [Space(10f)]
        [SerializeField] List<AdventureAreaElementUI> areaElementUIList = null;

        public new async void Initialize()
        {
            base.Initialize();
            await areaPopupUIPrefab.InitializeAsync();
            await upgradePopupUIPrefab.InitializeAsync();

            foreach (AdventureAreaElementUI elementUI in areaElementUIList)
                elementUI.Initialize(OpenUpgradePopupUI, OpenAreaPopupUI);
        }

        private void OpenUpgradePopupUI(int areaID)
        {
            AdventureAreaUpgradePopupUI upgradePopupUI = PoolManager.Spawn(upgradePopupUIPrefab, GameDefine.ContentPopupFrame);
            upgradePopupUI.StretchRect();
            upgradePopupUI.Initialize(areaID);
        }

        private void OpenAreaPopupUI(int areaID)
        {
            AdventureAreaPopupUI areaPopupUI = PoolManager.Spawn(areaPopupUIPrefab, GameDefine.ContentPopupFrame);
            areaPopupUI.StretchRect();
            areaPopupUI.Initialize(areaID);
        }

        public void OnTouchCloseButton()
        {
            base.Release();
            PoolManager.Despawn(this);
        }
    }
}
