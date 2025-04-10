using System;
using System.Collections.Generic;

namespace ProjectF.Datas
{
    public struct UserRepeatQuestDataChecker
    {
        public UserRepeatQuestDataChecker(UserData userData)
        {
            UserRepeatQuestData repeatQuestData = userData.repeatQuestData ??= new UserRepeatQuestData();
            repeatQuestData.repeatQuestDatas ??= new Dictionary<ERepeatQuestType, RepeatQuestData>();
            foreach(ERepeatQuestType repeatQuestType in Enum.GetValues<ERepeatQuestType>())
            {
                if(repeatQuestType == ERepeatQuestType.None)
                    continue;

                if(repeatQuestData.repeatQuestDatas.ContainsKey(repeatQuestType))
                    continue;

                repeatQuestData.repeatQuestDatas.Add(repeatQuestType, CreateNewRepeatQuestData());
            }
        }

        private RepeatQuestData CreateNewRepeatQuestData()
        {
            return new RepeatQuestData() {
                questID = 0,
                currentProgress = 0,
                repeatCount = 0
            };
        }
    }
}