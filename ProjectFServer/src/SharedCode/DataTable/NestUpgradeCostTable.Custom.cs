using System.Collections.Generic;

namespace ProjectF.DataTables
{
    public partial class NestUpgradeCostTableRow : UpgradeCostTableRow
    {
    }

    public partial class NestUpgradeCostTable : UpgradeCostTable<NestUpgradeCostTableRow> 
    { 
        private Dictionary<int, List<NestUpgradeCostTableRow>> tableRowListByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            tableRowListByLevel = new Dictionary<int, List<NestUpgradeCostTableRow>>();
            foreach (NestUpgradeCostTableRow tableRow in this)
            {
                if(tableRowListByLevel.TryGetValue(tableRow.level, out List<NestUpgradeCostTableRow> list) == false)
                {
                    list = new List<NestUpgradeCostTableRow>();
                    tableRowListByLevel.Add(tableRow.level, list);
                }

                list.Add(tableRow);
            }
        }

        public List<NestUpgradeCostTableRow> GetRowListByLevel(int level)
        {
            tableRowListByLevel.TryGetValue(level, out List<NestUpgradeCostTableRow> tableRow);
            return tableRow;
        }
    }
}
