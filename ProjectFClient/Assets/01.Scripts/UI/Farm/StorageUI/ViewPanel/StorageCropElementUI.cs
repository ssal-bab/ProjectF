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
        [SerializeField] Image itemIconImage = null;
        [SerializeField] Image gradeIconImage = null;
        [SerializeField] TMP_Text itemCountText = null;
        [SerializeField] TMP_Text priceText = null;

        private int id = 0;

        public void Initialize(int id, ECropGrade grade, int count)
        {
            this.id = id;

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

            RefreshUI(itemTableRow, cropTableRow, grade, count);
        }

        private void ResetUI()
        {
            itemIconImage.color = new Color(0, 0, 0, 0);
            gradeIconImage.color = new Color(0, 0, 0, 0);
            itemCountText.text = "";
            priceText.text = "";
        }

        private void RefreshUI(ItemTableRow itemTableRow, CropTableRow cropTableRow, ECropGrade grade, int count)
        {
            itemIconImage.sprite = ResourceUtility.GetItemIcon(itemTableRow.id);
            gradeIconImage.sprite = ResourceUtility.GetItemIcon((int)grade);
            itemCountText.text = $"{count} {itemTableRow.nameLocalKey}s";            
            priceText.text = (cropTableRow.basePrice * count).ToString();
        }

        public void OnTouchSellButton()
        {
            Debug.Log("[StorageCropElementUI::OnTouchSellButton] Sell Crop");
        }
    }
}