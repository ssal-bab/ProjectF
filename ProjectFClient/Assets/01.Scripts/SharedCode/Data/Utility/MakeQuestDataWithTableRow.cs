using System;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct MakeQuestDataWithTableRow
    {
        public QuestData questData;

        public MakeQuestDataWithTableRow(QuestTableRow tableRow)
        {
            Type type = Type.GetType($"ProjectF.Datas.{tableRow.questType}QuestData");
            questData = Activator.CreateInstance(type) as QuestData;
        }
    }
}