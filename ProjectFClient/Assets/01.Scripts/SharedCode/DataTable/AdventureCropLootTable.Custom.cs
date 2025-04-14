using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class AdventureCropLootTableRow : DataTableRow
    {
    }

    public partial class AdventureCropLootTable : DataTable<AdventureCropLootTableRow> 
    { 
        public Dictionary<int, Dictionary<int, List<AdventureCropLootTableRow>>> rowListByLevelByID = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            rowListByLevelByID = new Dictionary<int, Dictionary<int, List<AdventureCropLootTableRow>>>();
            foreach(AdventureCropLootTableRow tableRow in this)
            {
                if(rowListByLevelByID.TryGetValue(tableRow.adventureAreaID, out Dictionary<int, List<AdventureCropLootTableRow>> rowListByLevel) == false)
                {
                    rowListByLevel = new Dictionary<int, List<AdventureCropLootTableRow>>();
                    rowListByLevelByID.Add(tableRow.adventureAreaID, rowListByLevel);
                }

                if(rowListByLevel.TryGetValue(tableRow.adventureAreaLevel, out List<AdventureCropLootTableRow> list) == false)
                {
                    list = new List<AdventureCropLootTableRow>();
                    rowListByLevel.Add(tableRow.adventureAreaLevel, list);
                }

                list.Add(tableRow);
            }  
        }

        public List<AdventureCropLootTableRow> GetRowList(int adventureAreaID, int adventureAreaLevel)
        {
            if(rowListByLevelByID.TryGetValue(adventureAreaID, out Dictionary<int, List<AdventureCropLootTableRow>> rowListByLevel) == false)
                return null;

            rowListByLevel.TryGetValue(adventureAreaLevel, out List<AdventureCropLootTableRow> list);
            return list;
        }
    }
}
