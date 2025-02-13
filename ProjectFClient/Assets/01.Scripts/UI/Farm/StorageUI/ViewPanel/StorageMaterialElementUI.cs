using H00N.DataTables;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class StorageMaterialElementUI : MonoBehaviourUI
    {
        [SerializeField] Image itemIconImage = null;
        [SerializeField] TMP_Text itemNameText = null;
        [SerializeField] TMP_Text itemCountText = null;

        public void Initialize(int id, int count)
        {
            ItemTableRow itemTableRow = DataTableManager.GetTable<ItemTable>().GetRow(id);
            if(itemTableRow == null)
                return;

            RefreshUI(itemTableRow, count);
        }

        private void RefreshUI(ItemTableRow itemTableRow, int count)
        {
            itemIconImage.sprite = ResourceUtility.GetItemIcon(itemTableRow.id);
            itemNameText.text = itemTableRow.nameLocalKey;
            itemCountText.text = count.ToString();
        }

    }
}
