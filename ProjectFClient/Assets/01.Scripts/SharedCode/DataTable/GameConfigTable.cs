using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class GameConfigTableRow : ConfigTableRow<float>
    {
    }

    public partial class GameConfigTable : ConfigTable<GameConfigTableRow, float> { }
}
