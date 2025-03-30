using System;
using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public abstract partial class FacilityTableRow : DataTableRow
    {
        public int level;
        public int upgradeGold;
        public int skipGem;
        public int materialID;
        public int materialCount;
    }

    public abstract partial class FacilityTable<TRow> : DataTable<TRow> where TRow : FacilityTableRow { }
}