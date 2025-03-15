using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class CropTableRow : DataTableRow
    {
    }

    public partial class CropTable : DataTable<CropTableRow> 
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();
        }
    }
}