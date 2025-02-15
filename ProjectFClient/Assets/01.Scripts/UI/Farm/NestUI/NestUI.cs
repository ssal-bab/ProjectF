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

        #region Debug
        private void Start()
        {
            NestUICallbackContainer hi = null;
            hi = new NestUICallbackContainer(
                id => true,
                id => true,
                id => true,
                id => {
                    Debug.Log($"Upgrade Nest!! id : {id}");
                    Initialize(GameInstance.MainUser.nestData, hi);
                }
            );

            Initialize(GameInstance.MainUser.nestData, hi);
        }
        #endregion

        public void Initialize(UserNestData userNestData, NestUICallbackContainer callbackContainer)
        {
            Initialize();

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