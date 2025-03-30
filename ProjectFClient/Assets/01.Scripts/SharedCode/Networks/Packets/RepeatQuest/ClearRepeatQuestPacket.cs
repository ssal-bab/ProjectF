using System.Collections.Generic;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class ClearRepeatQuestRequest : RequestPacket
    {
        public override string Route => NetworkDefine.REPEAT_QUEST_ROUTE;

        public const string POST = "ClearRepeatQuest";
        public override string Post => POST;

        public ERepeatQuestType repeatQuestType;

        public ClearRepeatQuestRequest(ERepeatQuestType repeatQuestType)
        {
            this.repeatQuestType = repeatQuestType;
        }
    }

    public class ClearRepeatQuestResponse : ResponsePacket
    {
        public ERepeatQuestType repeatQuestType;
        public List<RewardData> rewardData;
        public RepeatQuestData newRepeatQuestData;
    }
}