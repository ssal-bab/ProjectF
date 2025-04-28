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
        [SerializeField] Image gradeIconImage = null;
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

            CropTableRow tableRow = DataTableManager.GetTable<CropTable>().GetRow(id);
            if(tableRow == null)
            {
                ResetUI();
                return;
            }

            RefreshUI(tableRow, count);
        }

        private void ResetUI()
        {
            iconImage.color = new Color(0, 0, 0, 0);
            new SetSprite(gradeIconImage, ResourceUtility.GetCropGradeIconKey((int)ECropGrade.None));
            countText.text = "";
            priceText.text = "";
        }

        private void RefreshUI(CropTableRow tableRow, int count)
        {
            new SetSprite(iconImage, ResourceUtility.GetCropIconKey(tableRow.id));
            new SetSprite(gradeIconImage, ResourceUtility.GetCropGradeIconKey((int)grade));
            countText.text = $"{count} {ResourceUtility.GetCropNameLocalKey(tableRow.id)}s";            
            priceText.text = (tableRow.basePrice * count).ToString();
        }

        public void OnTouchSellButton()
        {
            Debug.Log("[StorageCropElementUI::OnTouchSellButton] Sell Crop");
            sellCallback?.Invoke(this, id, grade);
        }
    }
}