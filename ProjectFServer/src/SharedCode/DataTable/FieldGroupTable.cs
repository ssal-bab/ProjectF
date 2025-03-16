using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class FieldGroupTableRow : FacilityTableRow
    {
        public float noneGradeRate;
        public float bronzeGradeRate;
        public float silverGradeRate;
        public float goldGradeRate;
    }

    public partial class FieldGroupTable : FacilityTable { }
}
