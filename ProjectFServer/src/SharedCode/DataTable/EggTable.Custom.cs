using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class EggTableRow : DataTableRow
    {
    }

    public partial class EggTable : DataTable<EggTableRow> 
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();
        }
    }
}