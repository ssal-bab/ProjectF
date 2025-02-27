using System;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class FarmerTableRow : DataTableRow
    {
        public ERariry rarity;
        public string nameLocalKey;
    }

    public partial class FarmerTable : DataTable<FarmerTableRow> { }
}