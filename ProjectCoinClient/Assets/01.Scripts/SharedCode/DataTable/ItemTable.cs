using System;
using H00N.DataTables;
using ProjectCoin.Datas;

namespace ProjectCoin.DataTables
{
    [Serializable]
    public class ItemTableRow : DataTableRow
    {
        public EItemType itemType;
        public string nameLocalKey;
    }

    public class ItemTable : DataTable<ItemTableRow> { }
}