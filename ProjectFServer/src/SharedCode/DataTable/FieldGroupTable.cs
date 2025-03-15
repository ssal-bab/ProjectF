using System;

namespace ProjectF.DataTables
{
    [Serializable]
    public partial class FieldGroupTableRow : FacilityTableRow
    {
        public float noneGradeRate;
        public float bronzeGradeRate;
        public float silverGradeRate;
        public float goldGradeRate;
    }

    public partial class FieldGroupTable : FacilityTable<FieldGroupTableRow> { }
}