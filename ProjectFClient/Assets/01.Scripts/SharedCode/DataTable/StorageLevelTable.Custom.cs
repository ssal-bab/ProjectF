using System.Collections.Generic;

namespace ProjectF.DataTables
{
    public partial class StorageLevelTableRow : LevelTableRow
    {
    }

    public partial class StorageLevelTable : LevelTable<StorageLevelTableRow> 
    { 
        private Dictionary<int, StorageLevelTableRow> rowByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            rowByLevel = new Dictionary<int, StorageLevelTableRow>();
            foreach (StorageLevelTableRow tableRow in this)
                rowByLevel[tableRow.level] = tableRow;
        }

        public StorageLevelTableRow GetRowByLevel(int level)
        {
            rowByLevel.TryGetValue(level, out StorageLevelTableRow tableRow);
            return tableRow;
        } 
    }
}