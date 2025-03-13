using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class NestPopupUI : PoolableBehaviourUI
    {
        [SerializeField] NestInfoPanelUI nestInfoPanel = null;
        [SerializeField] NestSlotListPanelUI nestSlotListPanel = null;

        public new void Initialize()
        {
            base.Initialize();
            nestInfoPanel.Initialize();
            nestSlotListPanel.Initialize();
        }

        protected override void Release()
        {
            base.Release();
        }
    }
}