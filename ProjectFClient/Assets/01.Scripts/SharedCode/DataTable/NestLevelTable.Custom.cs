using System.Collections.Generic;

namespace ProjectF.DataTables
{
    public partial class NestLevelTableRow : LevelTableRow
    {
    }

    public partial class NestLevelTable : LevelTable<NestLevelTableRow> 
    {
        private Dictionary<int, NestLevelTableRow> rowByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            rowByLevel = new Dictionary<int, NestLevelTableRow>();
            foreach (NestLevelTableRow tableRow in this)
                rowByLevel[tableRow.level] = tableRow;
        }

        public NestLevelTableRow GetRowByLevel(int level)
        {
            rowByLevel.TryGetValue(level, out NestLevelTableRow tableRow);
            return tableRow;
        } 
    }
}