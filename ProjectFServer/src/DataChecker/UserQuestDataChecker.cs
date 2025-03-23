using System;
using System.Collections.Generic;

namespace ProjectF.Datas
{
    public class UserQuestDataChecker
    {
        public UserQuestDataChecker(UserData userData)
        {
            UserQuestData questData = userData.questData ??= new UserQuestData();

            questData.quests ??= new Dictionary<int, QuestData>();
        }
    }
}