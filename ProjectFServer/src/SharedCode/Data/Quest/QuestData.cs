using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public class QuestData
    {
        public int id;
        public EQuestType questType;
        public string questName;
        public string message;
        public bool canClear;
        public string rewordType1;
        public int rewordAmount1;
        public string rewordType2;
        public int rewordAmount2;
        public string rewordType3;
        public int rewordAmount3;

        public QuestData() {}

        public QuestData(QuestTableRow tableRow)
        {
            id = tableRow.id;
            questType = tableRow.questType;
            questName = tableRow.questName;
            rewordType1 = tableRow.rewordType1;
            rewordType2 = tableRow.rewordType2;
            rewordType3 = tableRow.rewordType3;
            rewordAmount1 = tableRow.rewordAmount1;
            rewordAmount2 = tableRow.rewordAmount2;
            rewordAmount3 = tableRow.rewordAmount3;
        }
    }
}