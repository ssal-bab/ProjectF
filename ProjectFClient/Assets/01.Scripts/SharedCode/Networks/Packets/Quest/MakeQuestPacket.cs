using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF.Networks.Packets
{
    public class MakeQuestRequest : RequestPacket
    {
        public override string Route => NetworkDefine.QUEST_ROUTE;

        public const string POST = "MakeQuest";
        public override string Post => POST;

        public QuestData questData;

        public MakeQuestRequest(QuestData questData)
        {
            this.questData = questData;
        }
    }

    public class MakeQuestResponse : ResponsePacket
    {
        public QuestData questData;
    }
}