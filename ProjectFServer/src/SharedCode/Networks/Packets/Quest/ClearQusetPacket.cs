using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class ClearQuestRequest : RequestPacket
    {
        public override string Route => NetworkDefine.QUEST_ROUTE;

        public const string POST = "ClearQuest";
        public override string Post => POST;

        public QuestData questData;

        public ClearQuestRequest(QuestData questData)
        {
            this.questData = questData;
        }
    }

    public class ClearQuestResponse : ResponsePacket
    {
        public QuestData questData;
    }
}