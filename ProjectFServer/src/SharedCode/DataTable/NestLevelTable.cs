using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class NestLevelTableRow : LevelTableRow
    {
        public int eggStoreLimit;
        public int farmerStoreLimit;
    }

    public partial class NestLevelTable : LevelTable<NestLevelTableRow> { }
}
