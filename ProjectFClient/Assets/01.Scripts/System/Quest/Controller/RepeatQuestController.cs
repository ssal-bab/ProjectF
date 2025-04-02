using System.Collections;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Drawing.Diagrams;
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

                MakeQuest(pair.Key, tableRow, pair.Value);
            }
        }

        public override void Release()
        {
            repeatQuestDatas.Clear();
        }

        public RepeatQuest MakeQuest(ERepeatQuestType questType, RepeatQuestTableRow tableRow, RepeatQuestData questData)
        {
            repeatQuestDatas.Add(questType, new RepeatQuest(tableRow, questData, questType));

            return repeatQuestDatas[questType];
        }
    }
}
