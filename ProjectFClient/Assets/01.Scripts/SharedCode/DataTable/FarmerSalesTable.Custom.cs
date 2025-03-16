using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF
{
    public partial class FarmerSalesTableRow : DataTableRow
    {
    }

    public partial class FarmerSalesTable : DataTable<FarmerSalesTableRow> 
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();
        }
    }
}
