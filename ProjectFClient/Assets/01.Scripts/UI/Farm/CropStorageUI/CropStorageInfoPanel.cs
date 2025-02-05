using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.Farms
{
    public class CropStorageInfoPanel : MonoBehaviourUI
    {
        [SerializeField] Image storageIconImage = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text limitQuantityText = null;
        [SerializeField] TMP_Text usedQuantityText = null;

        private CropStorageTableRow storageData = null;
        private int usedQuantity = 0;
        
        public void Initialize(CropStorageTableRow storageData, int usedQuantity)
        {
            this.storageData = storageData;
            this.usedQuantity = usedQuantity;

            RefreshUI();
        }

        private void RefreshUI()
        {
            if(storageData == null)
                return;

            storageIconImage.sprite = new GetStorageIcon(storageData.id).sprite;
            nameText.text = $"Lv. {storageData.level} Storage{storageData.level}"; // 나중에 localizing 적용해야 함
            limitQuantityText.text = $"Max : {storageData.storeLimit}";
            usedQuantityText.text = $"Used : {usedQuantity}";
        }
    }
}
