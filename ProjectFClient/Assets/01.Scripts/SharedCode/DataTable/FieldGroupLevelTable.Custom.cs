using System.Collections.Generic;

namespace ProjectF.DataTables
{
    public partial class FieldGroupLevelTableRow : LevelTableRow
    {
    }

    public partial class FieldGroupLevelTable : LevelTable<FieldGroupLevelTableRow> 
    { 
        private Dictionary<int, FieldGroupLevelTableRow> rowByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            rowByLevel = new Dictionary<int, FieldGroupLevelTableRow>();
            foreach(FieldGroupLevelTableRow tableRow in this)
                rowByLevel[tableRow.level] = tableRow;
        }

        public FieldGroupLevelTableRow GetRowByLevel(int level)
        {
            rowByLevel.TryGetValue(level, out FieldGroupLevelTableRow tableRow);
            return tableRow;
        }
    }
}