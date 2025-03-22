using System.Collections;
using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF.DataTables
{
    public partial class FarmerSalesTableRow : DataTableRow
    {
    }

    public partial class FarmerSalesTable : DataTable<FarmerSalesTableRow> 
    {
        public float LevelSalesMultiplierValue { get; private set; }
        public float GradeSalesMultiplierValue { get; private set; }

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            var tableRow = this[0];
            
            LevelSalesMultiplierValue = tableRow.levelSalesMultiplierValue;
            GradeSalesMultiplierValue = tableRow.gradeSalesMultiplierValue;
        }
    }
}