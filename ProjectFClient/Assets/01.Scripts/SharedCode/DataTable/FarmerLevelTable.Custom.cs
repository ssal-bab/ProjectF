using System.Collections.Generic;

namespace ProjectF.DataTables
{
	public partial class FarmerLevelTable : LevelTable<FarmerLevelTableRow> 
    {
        private Dictionary<int, Dictionary<int, FarmerLevelTableRow>> rowByLevelByID = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            rowByLevelByID = new Dictionary<int, Dictionary<int, FarmerLevelTableRow>>();
            foreach (FarmerLevelTableRow tableRow in this)
            {
                if(rowByLevelByID.TryGetValue(tableRow.farmerID, out Dictionary<int, FarmerLevelTableRow> rowByLevel) == false)
                {
                    rowByLevel = new Dictionary<int, FarmerLevelTableRow>();
                    rowByLevelByID.Add(tableRow.farmerID, rowByLevel);
                }

                rowByLevel[tableRow.level] = tableRow;
            }
        }

        public FarmerLevelTableRow GetRow(int farmerID, int level)
        {
            if(rowByLevelByID.TryGetValue(farmerID, out var rowByLevel) == false)
                return null;

            rowByLevel.TryGetValue(level, out var tableRow);
            return tableRow;
        } 
    }
}