using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class FieldGroupTableRow : FacilityTableRow
    {
        public float[] rateTable;
        public float totalRates;
    }

    public partial class FieldGroupTable : FacilityTable<FieldGroupTableRow> 
    { 
        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            foreach(var tableRow in this)
            {
                tableRow.totalRates += tableRow.noneGradeRate + tableRow.bronzeGradeRate + tableRow.silverGradeRate + tableRow.goldGradeRate;
                tableRow.rateTable = new float[] {
                    tableRow.noneGradeRate,
                    tableRow.bronzeGradeRate,
                    tableRow.silverGradeRate,
                    tableRow.goldGradeRate
                };
            }
        }
    }
}