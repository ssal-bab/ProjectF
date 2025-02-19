using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class StorageInfoPanelUI : MonoBehaviourUI
    {
        public enum EStorageInfoUIType
        {
            Default,
            UpgradeCost,
            UpgradeMaterial,
            TOTAL_COUNT
        }

        [SerializeField] ToggleGroupUI infoUIToggleGroupUI = null;
        [SerializeField] StorageInfoUI[] storageInfoUIList = new StorageInfoUI[(int)EStorageInfoUIType.TOTAL_COUNT];

        private UserStorageData userStorageData = null;
        private StorageUICallbackContainer callbackContainer = null;

        public void Initialize(UserStorageData userStorageData, StorageUICallbackContainer callbackContainer)
        {
            base.Initialize();
            this.userStorageData = userStorageData;
            this.callbackContainer = callbackContainer;

            SetInfoUI(EStorageInfoUIType.Default);
        }

        public void SetInfoUI(EStorageInfoUIType infoUIType)
        {
            int targetIndex = (int)infoUIType;
            StorageInfoUI ui = storageInfoUIList[targetIndex];
            infoUIToggleGroupUI.SetToggle(ui);
            ui.Initialize(userStorageData, callbackContainer, this);
        }
    }
}
