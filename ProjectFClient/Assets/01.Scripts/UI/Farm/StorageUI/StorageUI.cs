using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class StorageUI : MonoBehaviourUI
    {
        private enum EStorageViewType
        {
            Crop,
            Material,
            TOTAL_COUNT
        }

        [SerializeField] StorageInfoPanel storageInfoPanel = null;
        [SerializeField] StorageViewPanel[] storageViewPanels = new StorageViewPanel[(int)EStorageViewType.TOTAL_COUNT];

        private UserStorageData userStorageData = null;
        private StorageUICallbackContainer callbackContainer = null;

        #region Debug
        private void Start()
        {
            StorageUICallbackContainer hi = null;
            hi = new StorageUICallbackContainer(
                id => Debug.Log($"Sell Crop!! id : {id}"),
                id => true,
                id => true,
                id => true,
                id => {
                    Debug.Log($"Upgrade Storage!! id : {id}");
                    Initialize(GameInstance.MainUser.storageData, hi);
                }
            );
            
            Initialize(GameInstance.MainUser.storageData, hi);
        }
        #endregion

        public void Initialize(UserStorageData userStorageData, StorageUICallbackContainer callbackContainer)
        {
            base.Initialize();
            this.userStorageData = userStorageData;
            this.callbackContainer = callbackContainer;

            storageInfoPanel.Initialize(this.userStorageData, this.callbackContainer);
            SetView(EStorageViewType.Crop);
        }

        protected override void Release()
        {
            base.Release();
            userStorageData = null;
        }

        public void OnTouchCropViewButton()
        {
            SetView(EStorageViewType.Crop);
        }

        public void OnTouchMaterialViewButton()
        {
            SetView(EStorageViewType.Material);
        }

        private void SetView(EStorageViewType viewType)
        {
            int targetIndex = (int)viewType;
            int totalCount = (int)EStorageViewType.TOTAL_COUNT;
            for(int i = 0; i < totalCount; ++i)
            {
                StorageViewPanel ui = storageViewPanels[i];
                if(i == targetIndex)
                {
                    ui.Initialize(userStorageData, callbackContainer);
                    ui.Show();
                }
                else
                {
                    ui.Hide();
                }
            }
        }
    }
}
