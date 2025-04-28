using H00N.DataTables;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Storages
{
    public class StorageSeedElementUI : PoolableBehaviourUI
    {
        [SerializeField] Image itemIconImage = null;
        [SerializeField] TMP_Text itemCountText = null;

        public void Initialize(int id, int count)
        {
            CropTableRow tableRow = DataTableManager.GetTable<CropTable>().GetRow(id);
            if(tableRow == null)
                return;

            RefreshUI(tableRow, count);
        }

        private void RefreshUI(CropTableRow tableRow, int count)
        {
            new SetSprite(itemIconImage, ResourceUtility.GetSeedIconKey(tableRow.id));
            itemCountText.text = count.ToString();
        }

    }
}
