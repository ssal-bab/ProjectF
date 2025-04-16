using System.Collections.Generic;

namespace ProjectF.DataTables
{
    public partial class FieldGroupLevelTableRow : LevelTableRow
    {
        public RatesData ratesData;
    }

    public partial class FieldGroupLevelTable : LevelTable<FieldGroupLevelTableRow> 
    { 
        private Dictionary<int, FieldGroupLevelTableRow> rowByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            rowByLevel = new Dictionary<int, FieldGroupLevelTableRow>();
            foreach(FieldGroupLevelTableRow tableRow in this)
            {
                float totalRate = tableRow.noneGradeRate + tableRow.bronzeGradeRate + tableRow.silverGradeRate + tableRow.goldGradeRate;
                float[] rates = new float[] { 
                    tableRow.noneGradeRate,
                    tableRow.bronzeGradeRate,
                    tableRow.silverGradeRate,
                    tableRow.goldGradeRate,
                };
                tableRow.ratesData = new RatesData(rates, totalRate);
                rowByLevel[tableRow.level] = tableRow;
            }
        }

        public FieldGroupLevelTableRow GetRowByLevel(int level)
        {
            rowByLevel.TryGetValue(level, out FieldGroupLevelTableRow tableRow);
            return tableRow;
        }
    }
}