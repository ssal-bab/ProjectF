using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public class FacilityTableRowBase : DataTableRow
    {
        public int upgradeGold;
        public int costItemID;
        public int costItemCount;
    }
}