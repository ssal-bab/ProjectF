using System;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class ItemTableRow : DataTableRow
    {
        public EItemType itemType;
        public string nameLocalKey;
    }

    public partial class ItemTable : DataTable<ItemTableRow> { }
}