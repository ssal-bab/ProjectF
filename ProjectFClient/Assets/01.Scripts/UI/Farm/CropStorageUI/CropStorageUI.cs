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
        [SerializeField] CropStorageViewPanel[] cropStorageViewPanels = new CropStorageViewPanel[2];

        private UserCropStorageData cropStorageData = null;

        private void Start()
        {
            Initialize(GameDefine.MainUser.cropStorageData);
        }

        public void Initialize(UserCropStorageData userCropStorageData)
        {
            Initialize();

            cropStorageData = userCropStorageData;            
            cropStorageInfoPanel.Initialize(cropStorageData);
            SetView(ECropStorageViewType.Crop);
        }

        protected override void Release()
        {
            base.Release();
            cropStorageData = null;
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
                    ui.Initialize(cropStorageData);
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
