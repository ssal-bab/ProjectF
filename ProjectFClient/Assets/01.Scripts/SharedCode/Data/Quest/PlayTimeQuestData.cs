using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public class PlayTimeQuestData : QuestData
    {
        public float targetTime;
        public float currentTime;

        public PlayTimeQuestData() {}

        public PlayTimeQuestData(QuestTableRow tableRow) : base(tableRow)
        {
            object[] parameters = new MakeQuestParams(tableRow).parameters;
            targetTime = (float)parameters[0];
        }
    }
}