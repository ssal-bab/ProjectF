using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class FarmerSalesTableRow : DataTableRow
    {
        public float levelSalesMultiplierValue;
        public float gradeSalesMultiplierValue;
    }

    public partial class FarmerSalesTable : DataTable<FarmerSalesTableRow> { }
}
