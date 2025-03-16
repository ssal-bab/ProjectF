using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class FarmerTableRow : DataTableRow
    {
        public ERarity rarity;
        public string nameLocalKey;
    }

    public partial class FarmerTable : DataTable<FarmerTableRow> { }
}
