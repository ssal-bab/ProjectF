using System;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    [Serializable]
    public class ItemTableRow : DataTableRow
    {
        public EItemType itemType;
        public string nameLocalKey;
    }

    public class ItemTable : DataTable<ItemTableRow> { }
}