namespace ProjectF.DataTables
{
    public partial class CropRepeatQuestTableRow : RepeatQuestTableRow
    {
        public float targetValueMultiplierByStorageLevel;
        public float rewardMultiplierByDefaultPrice;
    }

    public partial class CropRepeatQuestTable : RepeatQuestTable<CropRepeatQuestTableRow> { }
}