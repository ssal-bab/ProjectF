using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class NestInfoPanel : MonoBehaviourUI
    {
        public enum ENestInfoUIType
        {
            Default,
            UpgradeCost,
            UpgradeMaterial,
            TOTAL_COUNT
        }

        [SerializeField] ToggleGroupUI infoUIToggleGroupUI = null;
        [SerializeField] NestInfoUI[] nestInfoUIList = new NestInfoUI[(int)ENestInfoUIType.TOTAL_COUNT];

        private UserNestData userNestData = null;
        private NestUICallbackContainer callbackContainer = null;

        public void Initialize(UserNestData userNestData, NestUICallbackContainer callbackContainer)
        {
            this.userNestData = userNestData;
            this.callbackContainer = callbackContainer;

            SetInfoUI(ENestInfoUIType.Default);
        }

        public void SetInfoUI(ENestInfoUIType infoUIType)
        {
            int targetIndex = (int)infoUIType;
            NestInfoUI ui = nestInfoUIList[targetIndex];
            infoUIToggleGroupUI.SetToggle(ui);
            ui.Initialize(userNestData, callbackContainer, this);
        }
    }
}