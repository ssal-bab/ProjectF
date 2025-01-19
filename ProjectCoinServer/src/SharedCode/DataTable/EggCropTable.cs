using System;
using H00N.DataTables;

namespace ProjectCoin.DataTables
{
    [Serializable]
    public class EggCropTableRow : DataTableRow
    {
        public int itemID;
    }

    public class EggCropTable : DataTable<EggCropTableRow> { }
}