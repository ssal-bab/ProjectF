using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class AdventureAreaTableRow : DataTableRow
    {
        public string nameLocalKey;
        public int costItem1;
        public int costValue1;
        public int costItem2;
        public int costValue2;
        public int costItem3;
        public int costValue3;
        public int costItem4;
        public int costValue4;
        public int upgradeCost;
    }

    public partial class AdventureAreaTable : DataTable<AdventureAreaTableRow> { }
}
