using H00N.DataTables;

namespace ProjectF.DataTables
{
	public partial class FarmerStatTableRow : DataTableRow
	{
    }

	public partial class FarmerStatTable : DataTable<FarmerStatTableRow> 
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();
        }
    }
}