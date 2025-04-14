using System.Collections.Generic;

namespace ProjectF.DataTables
{
    public partial class AdventureUpgradeCostTableRow : UpgradeCostTableRow
    {
    }

    public partial class AdventureUpgradeCostTable : UpgradeCostTable<AdventureUpgradeCostTableRow> 
    { 
        private Dictionary<int, Dictionary<int, List<AdventureUpgradeCostTableRow>>> rowListByLevelByID = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            rowListByLevelByID = new Dictionary<int, Dictionary<int, List<AdventureUpgradeCostTableRow>>>();
            foreach (AdventureUpgradeCostTableRow tableRow in this)
            {
                if(rowListByLevelByID.TryGetValue(tableRow.adventureAreaID, out Dictionary<int, List<AdventureUpgradeCostTableRow>> rowListByLevel) == false)
                {
                    rowListByLevel = new Dictionary<int, List<AdventureUpgradeCostTableRow>>();
                    rowListByLevelByID.Add(tableRow.adventureAreaID, rowListByLevel);
                }

                if(rowListByLevel.TryGetValue(tableRow.level, out List<AdventureUpgradeCostTableRow> list) == false)
                {
                    list = new List<AdventureUpgradeCostTableRow>();
                    rowListByLevel.Add(tableRow.level, list);
                }

                list.Add(tableRow);
            }
        }

        public List<AdventureUpgradeCostTableRow> GetRow(int adventureAreaID, int level)
        {
            if(rowListByLevelByID.TryGetValue(adventureAreaID, out var rowListByLevel) == false)
                return null;

            rowListByLevel.TryGetValue(adventureAreaID, out var list);
            return list;
        } 
    }
}
