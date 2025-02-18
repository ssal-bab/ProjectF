using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class FieldGroupTableRow : FacilityTableRowBase
    {
        public float[] rateTable;
        public float totalRates;
    }

    public partial class FieldGroupTable : DataTable<FieldGroupTableRow> 
    { 
        private Dictionary<int, FieldGroupTableRow> tableByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            tableByLevel = new Dictionary<int, FieldGroupTableRow>();
            foreach(var tableRow in table.Values)
            {
                tableRow.totalRates += tableRow.noneGradeRate + tableRow.bronzeGradeRate + tableRow.silverGradeRate + tableRow.goldGradeRate;
                tableRow.rateTable = new float[] {
                    tableRow.noneGradeRate,
                    tableRow.bronzeGradeRate,
                    tableRow.silverGradeRate,
                    tableRow.goldGradeRate
                };

                if(tableByLevel.ContainsKey(tableRow.level))
                    continue;

                tableByLevel.Add(tableRow.level, tableRow);
            }
        }

        public FieldGroupTableRow GetRowByLevel(int level)
        {
            tableByLevel.TryGetValue(level, out FieldGroupTableRow tableRow);
            return tableRow;
        }
    }
}