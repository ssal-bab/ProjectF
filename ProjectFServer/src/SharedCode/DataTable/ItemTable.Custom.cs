using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class ItemTableRow : DataTableRow
    {
    }

    public partial class ItemTable : DataTable<ItemTableRow> 
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();
        }
    }
}