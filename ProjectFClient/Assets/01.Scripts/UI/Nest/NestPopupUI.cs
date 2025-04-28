using H00N.Resources.Pools;
using UnityEngine;

namespace ProjectF.UI.Nests
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

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
        }
    }
}