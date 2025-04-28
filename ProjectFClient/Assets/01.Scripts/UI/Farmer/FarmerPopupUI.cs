using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using TMPro;
using UnityEngine;

namespace ProjectF.UI.Farmers
{
    public partial class FarmerPopupUI : PoolableBehaviourUI
    {
        [SerializeField] TMP_Text titleText = null;
        [SerializeField] GameObject sellButtonObject = null;

        [Space(10f)]
        [SerializeField] AddressableAsset<FarmerElementUI> farmerElementUIPrefab = null;
        [SerializeField] Transform container = null;

        private Color selectedColor = Color.white;

        public new async void Initialize()
        {
            base.Initialize();
            await farmerElementUIPrefab.InitializeAsync();

            RefreshUI();
        }

        private void RefreshUI()
        {
            container.DespawnAllChildren();
            UserFarmerData userFarmerData = GameInstance.MainUser.farmerData;
            foreach(var farmerData in userFarmerData.farmerDatas.Values)
            {
                FarmerElementUI ui = PoolManager.Spawn<FarmerElementUI>(farmerElementUIPrefab, container);
                ui.Initialize(farmerData.farmerUUID, selectedColor);
            }
        }
    }
}
