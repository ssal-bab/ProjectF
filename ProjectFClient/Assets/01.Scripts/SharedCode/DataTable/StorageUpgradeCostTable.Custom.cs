using System.Collections.Generic;

namespace ProjectF.DataTables
{
    public partial class StorageUpgradeCostTableRow : UpgradeCostTableRow
    {
    }

    public partial class StorageUpgradeCostTable : UpgradeCostTable<StorageUpgradeCostTableRow> 
    {
        private Dictionary<int, List<StorageUpgradeCostTableRow>> tableRowListByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            tableRowListByLevel = new Dictionary<int, List<StorageUpgradeCostTableRow>>();
            foreach (StorageUpgradeCostTableRow tableRow in this)
            {
                if(tableRowListByLevel.TryGetValue(tableRow.level, out List<StorageUpgradeCostTableRow> list) == false)
                {
                    list = new List<StorageUpgradeCostTableRow>();
                    tableRowListByLevel.Add(tableRow.level, list);
                }

                list.Add(tableRow);
            }
        }

        public List<StorageUpgradeCostTableRow> GetRowListByLevel(int level)
        {
            tableRowListByLevel.TryGetValue(level, out List<StorageUpgradeCostTableRow> tableRow);
            return tableRow;
        }
    }
}
