using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class CropTableRow : DataTableRow
    {
        public int growthStep;
        public int growthRate;
        public int basePrice;
    }

    public partial class CropTable : DataTable<CropTableRow> { }
}