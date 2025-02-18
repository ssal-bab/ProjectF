using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class CropTableRow : DataTableRow
    {
        public int seedItemID;
        public int cropItemID;
        public int growthStep;
        public int growthRate;
        public int basePrice;
        public string nameLocalKey;
    }

    public partial class CropTable : DataTable<CropTableRow> { }
}