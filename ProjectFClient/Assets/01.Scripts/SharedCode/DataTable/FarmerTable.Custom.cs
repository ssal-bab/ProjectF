using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class FarmerTableRow : DataTableRow
    {
    }

    public partial class FarmerTable : DataTable<FarmerTableRow> 
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();
        }
    }
}