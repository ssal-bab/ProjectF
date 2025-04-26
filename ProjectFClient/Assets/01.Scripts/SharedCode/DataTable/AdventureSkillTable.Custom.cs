using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class AdventureSkillTable : DataTable<AdventureSkillTableRow> 
    { 
        private Dictionary<int, AdventureSkillTableRow> tableByLevel;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            tableByLevel = new Dictionary<int, AdventureSkillTableRow>();
            foreach(AdventureSkillTableRow row in this)
                tableByLevel.Add(row.level, row);
        }

        public AdventureSkillTableRow GetRowByLevel(int level)
        {
            tableByLevel.TryGetValue(level, out AdventureSkillTableRow row);
            return row;
        }
    }
}
