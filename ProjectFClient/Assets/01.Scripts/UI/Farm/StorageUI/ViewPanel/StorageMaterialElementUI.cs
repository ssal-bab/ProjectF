using H00N.DataTables;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class StorageMaterialElementUI : PoolableBehaviourUI
    {
        [SerializeField] Image itemIconImage = null;
        [SerializeField] TMP_Text itemNameText = null;
        [SerializeField] TMP_Text itemCountText = null;

        public void Initialize(int id, int count)
        {
            MaterialTableRow tableRow = DataTableManager.GetTable<MaterialTable>().GetRow(id);
            if(tableRow == null)
                return;

            RefreshUI(tableRow, count);
        }

        private void RefreshUI(MaterialTableRow tableRow, int count)
        {
            itemIconImage.sprite = ResourceUtility.GetMaterialIcon(tableRow.id);
            itemNameText.text = ResourceUtility.GetMaterialNameLocalKey(tableRow.id);
            itemCountText.text = count.ToString();
        }

    }
}
