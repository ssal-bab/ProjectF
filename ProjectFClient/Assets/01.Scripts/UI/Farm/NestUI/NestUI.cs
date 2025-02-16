using Cysharp.Threading.Tasks;
using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class NestUI : MonoBehaviourUI
    {
        [SerializeField] NestInfoPanel nestInfoPanel = null;
        [SerializeField] NestSlotListPanel nestSlotListPanel = null;

        private UserNestData userNestData = null;
        private NestUICallbackContainer callbackContainer = null;

        public void Initialize(UserNestData userNestData, NestUICallbackContainer callbackContainer)
        {
            base.Initialize();
            this.userNestData = userNestData;
            this.callbackContainer = callbackContainer;

            nestInfoPanel.Initialize(this.userNestData, this.callbackContainer);
            nestSlotListPanel.Initialize(this.userNestData, this.callbackContainer);
        }

        protected override void Release()
        {
            base.Release();
            userNestData = null;
        }
    }
}