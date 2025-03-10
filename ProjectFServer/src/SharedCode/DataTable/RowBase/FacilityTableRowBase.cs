using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public class FacilityTableRowBase : DataTableRow
    {
        public int upgradeGold;
        public int skipGem;
        public int materialID;
        public int materialCount;
    }
}