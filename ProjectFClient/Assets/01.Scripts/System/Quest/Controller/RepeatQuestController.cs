using System.Collections;
using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Quests;
using UnityEngine;

namespace ProjectF
{
    public class RepeatQuestController : QuestController
    {
        public Dictionary<ERepeatQuestType, RepeatQuest> repeatQuestDatas = null;

        public override void Initialize()
        {
            repeatQuestDatas = new();
            UserRepeatQuestData repeatQuestData = GameInstance.MainUser.repeatQuestData;
            foreach(var pair in repeatQuestData.repeatQuestDatas)
            {
                if(pair.Key == ERepeatQuestType.None)
                    continue;

                RepeatQuestTableRow tableRow = null;
                switch(pair.Key)
                {
                    case ERepeatQuestType.Crop:
                        tableRow = DataTableManager.GetTable<CropRepeatQuestTable>().GetRow(pair.Value.questID);
                        break;
                    case ERepeatQuestType.Farm:
                        tableRow = DataTableManager.GetTable<FarmRepeatQuestTable>().GetRow(pair.Value.questID);
                        break;
                    case ERepeatQuestType.Adventure:
                        tableRow = DataTableManager.GetTable<AdventureRepeatQuestTable>().GetRow(pair.Value.questID);
                        break;
                }
                repeatQuestDatas.Add(pair.Key, new RepeatQuest(tableRow, pair.Key));

                MakeQuest(repeatQuestDatas[pair.Key], !repeatQuestData.repeatQuestDatas[pair.Key].started, true);
            }
        }

        public override void Release()
        {
            repeatQuestDatas.Clear();
        }
    }
}
