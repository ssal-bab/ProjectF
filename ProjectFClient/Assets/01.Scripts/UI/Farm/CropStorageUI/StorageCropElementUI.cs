using System;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class StorageCropElementUI : MonoBehaviourUI
    {
        [SerializeField] Image itemIconImage = null;
        [SerializeField] Image gradeIconImage = null;
        [SerializeField] TMP_Text itemCountText = null;
        [SerializeField] TMP_Text priceText = null;

        private int id = 0;
        private Action<int> sellCropCallback = null;

        public void Initialize(int id, ECropGrade grade, int count, Action<int> sellCropCallback)
        {
            this.id = id;
            this.sellCropCallback = sellCropCallback;

            ItemTableRow itemTableRow = DataTableManager.GetTable<ItemTable>().GetRow(id);
            if(itemTableRow == null)
                return;

            CropTableRow cropTableRow = DataTableManager.GetTable<CropTable>().GetRowByProductID(id);
            if(cropTableRow == null)
                return;

            RefreshUI(itemTableRow, cropTableRow, grade, count);
        }

        private void RefreshUI(ItemTableRow itemTableRow, CropTableRow cropTableRow, ECropGrade grade, int count)
        {
            itemIconImage.sprite = ResourceUtility.GetItemIcon(itemTableRow.id);
            gradeIconImage.sprite = ResourceUtility.GetItemIcon((int)grade);
            itemCountText.text = $"{count} {itemTableRow.nameLocalKey}s";            
            priceText.text = (cropTableRow.basePrice * count).ToString();
        }

        public void OnSellButtonClicked()
        {
            sellCropCallback?.Invoke(id);
        }
    }
}