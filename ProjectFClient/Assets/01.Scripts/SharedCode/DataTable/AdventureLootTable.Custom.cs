using System.Collections;
using System.Collections.Generic;
using H00N.DataTables;
using UnityEngine;

namespace ProjectF.DataTables
{
    public partial class AdventureLootTableRow : DataTableRow
    {
    }

    public partial class AdventureLootTable : DataTable<AdventureLootTableRow>
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();
        }
    }
}
