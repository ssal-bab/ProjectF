using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class CropStorageInfoPanel : MonoBehaviourUI
    {
        [SerializeField] Image storageIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text limitCountText = null;
        [SerializeField] TMP_Text usedCountText = null;

        private CropStorageTableRow storageData = null;
        private int usedCount = 0;
        
        public void Initialize(UserCropStorageData userCropStorageData)
        {
            CropStorageTable cropStorageTable = DataTableManager.GetTable<CropStorageTable>();
            storageData = cropStorageTable.GetRowByLevel(userCropStorageData.level);;
            usedCount = new GetCropStorageUsedCount(userCropStorageData).usedCount;

            RefreshUI();
        }

        private void RefreshUI()
        {
            if(storageData == null)
                return;

            storageIconImage.sprite = new GetStorageIcon(storageData.id).sprite;
            nameText.text = $"Lv. {storageData.level} Storage{storageData.level}"; // 나중에 localizing 적용해야 함
            limitCountText.text = $"Max : {storageData.storeLimit}";
            usedCountText.text = $"Used : {usedCount}";
        }
    }
}
