using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class CropStorageInfoPanel : MonoBehaviourUI
    {
        public enum ECropStorageInfoUIType
        {
            Default,
            UpgradeCost,
            UpgradeMaterial,
            TOTAL_COUNT
        }

        [SerializeField] ToggleGroupUI infoUIToggleGroupUI = null;
        [SerializeField] CropStorageInfoUI[] cropStorageInfoUIList = new CropStorageInfoUI[(int)ECropStorageInfoUIType.TOTAL_COUNT];

        private UserCropStorageData userCropStorageData = null;
        private CropStorageUICallbackContainer callbackContainer = null;

        public void Initialize(UserCropStorageData userCropStorageData, CropStorageUICallbackContainer callbackContainer)
        {
            this.userCropStorageData = userCropStorageData;
            this.callbackContainer = callbackContainer;

            SetInfoUI(ECropStorageInfoUIType.Default);
        }

        public void SetInfoUI(ECropStorageInfoUIType infoUIType)
        {
            int targetIndex = (int)infoUIType;
            CropStorageInfoUI ui = cropStorageInfoUIList[targetIndex];
            infoUIToggleGroupUI.SetToggle(ui);
            ui.Initialize(userCropStorageData, callbackContainer, this);
        }
    }
}
