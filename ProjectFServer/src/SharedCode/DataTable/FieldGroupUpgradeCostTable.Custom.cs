using System.Collections.Generic;

namespace ProjectF.DataTables
{
    public partial class FieldGroupUpgradeCostTableRow : UpgradeCostTableRow
    {
    }

    public partial class FieldGroupUpgradeCostTable : UpgradeCostTable<FieldGroupUpgradeCostTableRow> 
    { 
        private Dictionary<int, List<FieldGroupUpgradeCostTableRow>> tableRowListByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            tableRowListByLevel = new Dictionary<int, List<FieldGroupUpgradeCostTableRow>>();
            foreach (FieldGroupUpgradeCostTableRow tableRow in this)
            {
                if(tableRowListByLevel.TryGetValue(tableRow.level, out List<FieldGroupUpgradeCostTableRow> list) == false)
                {
                    list = new List<FieldGroupUpgradeCostTableRow>();
                    tableRowListByLevel.Add(tableRow.level, list);
                }

                list.Add(tableRow);
            }
        }

        public List<FieldGroupUpgradeCostTableRow> GetRowListByLevel(int level)
        {
            tableRowListByLevel.TryGetValue(level, out List<FieldGroupUpgradeCostTableRow> tableRow);
            return tableRow;
        }
    }
}
