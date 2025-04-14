using System.Collections.Generic;

namespace ProjectF.DataTables
{
    public partial class AdventureLevelTableRow : LevelTableRow
    {
    }

    public partial class AdventureLevelTable : LevelTable<AdventureLevelTableRow> 
    { 
        private Dictionary<int, Dictionary<int, AdventureLevelTableRow>> rowByLevelByID = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            rowByLevelByID = new Dictionary<int, Dictionary<int, AdventureLevelTableRow>>();
            foreach (AdventureLevelTableRow tableRow in this)
            {
                if(rowByLevelByID.TryGetValue(tableRow.adventureAreaID, out Dictionary<int, AdventureLevelTableRow> randomIndexowByLevel) == false)
                {
                    randomIndexowByLevel = new Dictionary<int, AdventureLevelTableRow>();
                    rowByLevelByID.Add(tableRow.adventureAreaID, randomIndexowByLevel);
                }

                randomIndexowByLevel[tableRow.level] = tableRow;
            }
        }

        public AdventureLevelTableRow GetRow(int adventureAreaID, int level)
        {
            if(rowByLevelByID.TryGetValue(adventureAreaID, out var rowByLevel) == false)
                return null;

            rowByLevel.TryGetValue(level, out var tableRow);
            return tableRow;
        } 
    }
}
