namespace ProjectF.DataTables
{
    public partial class FieldGroupLevelTableRow : LevelTableRow
    {
        public float noneGradeRate;
        public float bronzeGradeRate;
        public float silverGradeRate;
        public float goldGradeRate;
    }

    public partial class FieldGroupLevelTable : LevelTable<FieldGroupLevelTableRow> { }
}
