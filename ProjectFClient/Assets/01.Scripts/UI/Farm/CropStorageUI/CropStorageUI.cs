using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class CropStorageUI : MonoBehaviourUI
    {
        private enum ECropStorageViewType
        {
            Crop,
            Material,
            TOTAL_COUNT
        }

        [SerializeField] CropStorageInfoPanel cropStorageInfoPanel = null;
        [SerializeField] CropStorageViewPanel[] cropStorageViewPanels = new CropStorageViewPanel[(int)ECropStorageViewType.TOTAL_COUNT];

        private UserCropStorageData userCropStorageData = null;
        private CropStorageUICallbackContainer callbackContainer = null;

        #region Debug
        private void Start()
        {
            CropStorageUICallbackContainer hi = null;
            hi = new CropStorageUICallbackContainer(
                id => Debug.Log($"Sell Crop!! id : {id}"),
                id => true,
                id => true,
                id => true,
                id => {
                    Debug.Log($"Upgrade Storage!! id : {id}");
                    Initialize(GameDefine.MainUser.cropStorageData, hi);
                }
            );
            
            Initialize(GameDefine.MainUser.cropStorageData, hi);
        }
        #endregion

        public void Initialize(UserCropStorageData userCropStorageData, CropStorageUICallbackContainer callbackContainer)
        {
            Initialize();

            this.userCropStorageData = userCropStorageData;
            this.callbackContainer = callbackContainer;

            cropStorageInfoPanel.Initialize(this.userCropStorageData, this.callbackContainer);
            SetView(ECropStorageViewType.Crop);
        }

        protected override void Release()
        {
            base.Release();
            userCropStorageData = null;
        }

        public void OnTouchCropViewButton()
        {
            SetView(ECropStorageViewType.Crop);
        }

        public void OnTouchMaterialViewButton()
        {
            SetView(ECropStorageViewType.Material);
        }

        private void SetView(ECropStorageViewType viewType)
        {
            int targetIndex = (int)viewType;
            int totalCount = (int)ECropStorageViewType.TOTAL_COUNT;
            for(int i = 0; i < totalCount; ++i)
            {
                CropStorageViewPanel ui = cropStorageViewPanels[i];
                if(i == targetIndex)
                {
                    ui.Initialize(userCropStorageData, callbackContainer);
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
