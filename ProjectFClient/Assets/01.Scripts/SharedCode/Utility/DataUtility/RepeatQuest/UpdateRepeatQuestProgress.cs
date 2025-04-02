using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct UpdateRepeatQuestProgress
    {
        public int newProgress;

        public UpdateRepeatQuestProgress(UserData userData, ERepeatQuestType questType, EActionType actionType, int updateValue, int targetID = -1)
        {
            newProgress = -1;

            RepeatQuestData questData = userData.repeatQuestData.repeatQuestDatas[questType];
            RepeatQuestTableRow tableRow;
            switch(questType)
            {
                case ERepeatQuestType.Crop:
                    tableRow = DataTableManager.GetTable<CropRepeatQuestTable>().GetRow(questData.questID);
                break;
                case ERepeatQuestType.Farm:
                    tableRow = DataTableManager.GetTable<FarmRepeatQuestTable>().GetRow(questData.questID);
                break;
                case ERepeatQuestType.Adventure:
                    tableRow = DataTableManager.GetTable<AdventureRepeatQuestTable>().GetRow(questData.questID);
                break;
                default:
                    tableRow = null;
                break;
            }

            if(tableRow != null && tableRow.actionType == actionType)
            {
                switch(tableRow.actionType)
                {
                    case EActionType.HarvestCrop:
                        questData.currentProgress += updateValue;
                    break;
                    case EActionType.HarvestTargetCrop:
                        if(questData.actionTargetID == targetID)
                        {
                            questData.currentProgress += updateValue;
                        }
                    break;
                    case EActionType.HatchEgg:
                        questData.currentProgress += updateValue;
                    break;
                    case EActionType.HatchTargetEgg:
                        if(questData.actionTargetID == targetID)
                        {
                            questData.currentProgress += updateValue;
                        }
                    break;
                    case EActionType.OwnCrop:
                        questData.currentProgress += updateValue;
                    break;
                    case EActionType.OwnTargetCrop:
                        if(questData.actionTargetID == targetID)
                        {
                            questData.currentProgress += updateValue;
                        }
                    break;
                    case EActionType.PlantSeed:
                        questData.currentProgress += updateValue;
                    break;
                    case EActionType.PlantTargetSeed:
                        if(questData.actionTargetID == targetID)
                        {
                            questData.currentProgress += updateValue;
                        }
                    break;
                    case EActionType.AdventureComplete:
                        questData.currentProgress += updateValue;
                    break;
                    case EActionType.TargetAdventureComplete:
                        if(questData.actionTargetID == targetID)
                        {
                            questData.currentProgress += updateValue;
                        }
                    break;
                }

                newProgress = questData.currentProgress;
            }
        }
    }
}