using H00N.Extensions;
using H00N.Resources.Pools;
using UnityEngine;

namespace ProjectF.UI.Storages
{
    public class StoragePopupUI : PoolableBehaviourUI
    {
        private enum EStorageViewType
        {
            Crop,
            Material,
            TOTAL_COUNT
        }

        [SerializeField] StorageInfoPanelUI storageInfoPanel = null;
        [SerializeField] StorageViewPanelUI[] storageViewPanels = new StorageViewPanelUI[(int)EStorageViewType.TOTAL_COUNT];

        public new void Initialize()
        {
            base.Initialize();

            storageInfoPanel.Initialize();
            SetView(EStorageViewType.Crop);
        }

        protected override void Release()
        {
            base.Release();
            storageInfoPanel.Release();
            storageViewPanels.ForEach(i => i.Release());
        }

        public void OnTouchCropViewButton()
        {
            SetView(EStorageViewType.Crop);
        }

        public void OnTouchMaterialViewButton()
        {
            SetView(EStorageViewType.Material);
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
        }

        private void SetView(EStorageViewType viewType)
        {
            int targetIndex = (int)viewType;
            int totalCount = (int)EStorageViewType.TOTAL_COUNT;
            for(int i = 0; i < totalCount; ++i)
            {
                StorageViewPanelUI ui = storageViewPanels[i];
                if(i == targetIndex)
                {
                    ui.Initialize();
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
