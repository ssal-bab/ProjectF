using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct GetRepeatQuestTableRow
    {
        public RepeatQuestTableRow tableRow;

        public GetRepeatQuestTableRow(ERepeatQuestType repeatQuestType, int id)
        {
            tableRow = null;

            switch(repeatQuestType)
            {
                case ERepeatQuestType.Crop:
                    tableRow = DataTableManager.GetTable<CropRepeatQuestTable>().GetRow(id);
                    break;
                case ERepeatQuestType.Farm:
                    tableRow = DataTableManager.GetTable<FarmRepeatQuestTable>().GetRow(id);
                    break;
                case ERepeatQuestType.Adventure:
                    tableRow = DataTableManager.GetTable<AdventureRepeatQuestTable>().GetRow(id);
                    break;
            }
        }
    }
}