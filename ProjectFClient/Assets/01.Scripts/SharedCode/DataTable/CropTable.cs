using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class CropTableRow : DataTableRow
    {
        public int growthStep;
        public int growthRate;
        public int basePrice;
    }

    public partial class CropTable : DataTable<CropTableRow> { }
}
