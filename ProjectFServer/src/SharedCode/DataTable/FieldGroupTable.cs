using System;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class FieldGroupTableRow : FacilityTableRowBase
    {
        public int level;
        public float noneGradeRate;
        public float bronzeGradeRate;
        public float silverGradeRate;
        public float goldGradeRate;
    }

    public partial class FieldGroupTable : DataTable<FieldGroupTableRow> { }
}