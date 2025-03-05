using System;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class StorageCropElementUI : PoolableBehaviourUI
    {
        [SerializeField] Image iconImage = null;
        [SerializeField] Image iconBackgroundImage = null;
        [SerializeField] TMP_Text countText = null;
        [SerializeField] TMP_Text priceText = null;

        private int id = 0;
        private ECropGrade grade = ECropGrade.None;
        private Action<StorageCropElementUI, int, ECropGrade> sellCallback = null;

        public void Initialize(int id, ECropGrade grade, int count, Action<StorageCropElementUI, int, ECropGrade> sellCallback)
        {
            this.id = id;
            this.grade = grade;
            this.sellCallback = sellCallback;

            ItemTableRow itemTableRow = DataTableManager.GetTable<ItemTable>().GetRow(id);
            if(itemTableRow == null)
            {
                ResetUI();
                return;
            }

            CropTableRow cropTableRow = DataTableManager.GetTable<CropTable>().GetRowByProductID(id);
            if(cropTableRow == null)
            {
                ResetUI();
                return;
            }

            RefreshUI(itemTableRow, cropTableRow, count);
        }

        private void ResetUI()
        {
            iconImage.color = new Color(0, 0, 0, 0);
            iconBackgroundImage.sprite = ResourceUtility.GetItemIcon((int)ECropGrade.None);
            countText.text = "";
            priceText.text = "";
        }

        private void RefreshUI(ItemTableRow itemTableRow, CropTableRow cropTableRow, int count)
        {
            iconImage.sprite = ResourceUtility.GetItemIcon(itemTableRow.id);
            iconBackgroundImage.sprite = ResourceUtility.GetItemIcon((int)grade);
            countText.text = $"{count} {itemTableRow.nameLocalKey}s";            
            priceText.text = (cropTableRow.basePrice * count).ToString();
        }

        public void OnTouchSellButton()
        {
            Debug.Log("[StorageCropElementUI::OnTouchSellButton] Sell Crop");
            sellCallback?.Invoke(this, id, grade);
        }
    }
}