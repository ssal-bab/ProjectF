using System;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct UpdateAllQuestDataProgress
    {
        public UpdateAllQuestDataProgress(UserData userData, EActionType actionType, int actionTargetID, int progress)
        {
            new UpdateRepeateQuestDataProgress(userData.repeatQuestData, actionType, actionTargetID, progress);
        }
    }

    public struct UpdateRepeateQuestDataProgress
    {
        public UpdateRepeateQuestDataProgress(UserRepeatQuestData repeatQuestData, EActionType actionType, int actionTargetID, int progress)
        {
            foreach(ERepeatQuestType repeatQuestType in Enum.GetValues(typeof(ERepeatQuestType)))
            {
                if(repeatQuestType == ERepeatQuestType.None)
                    continue;

                if(repeatQuestData.repeatQuestDatas.TryGetValue(repeatQuestType, out RepeatQuestData data) == false)
                    continue;

                if(data.actionTargetID != DataDefine.NONE_TARGET && data.actionTargetID != actionTargetID)
                    continue;

                RepeatQuestTableRow tableRow = new GetRepeatQuestTableRow(repeatQuestType, data.questID).tableRow;
                if(tableRow.actionType != actionType)
                    continue;

                data.currentProgress += progress;
            }
        }
    }
}