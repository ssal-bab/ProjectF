using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.Packets;
using ProjectF.Networks;
using UnityEngine;

namespace ProjectF.Quests
{
    public class RepeatQuest : Quest
    {
        private ERepeatQuestType repeatQuestType;
        public ERepeatQuestType RepeatQuestType => repeatQuestType;

        public RepeatQuest(RepeatQuestTableRow tableRow, RepeatQuestData questData,  ERepeatQuestType repeatQuestType) : base(tableRow, questData)
        {
            this.repeatQuestType = repeatQuestType;

            SetDescription();
        }

        protected override async void CheckClear()
        {
            Debug.Log($"check clear quest : {repeatQuestType}{QuestData.questID}");

            ClearRepeatQuestRequest req = new ClearRepeatQuestRequest(repeatQuestType);
            ClearRepeatQuestResponse res = await NetworkManager.Instance.SendWebRequestAsync<ClearRepeatQuestResponse>(req);
            if(res.result != ENetworkResult.Success)
            {
                return;
            }
                
            Debug.Log($"clear repeat quest : {repeatQuestType}");

            if(!TableRow.actionType.ToString().Contains("Target"))
                UserActionObserver.UnregistObserver(TableRow.actionType, CheckClear);
            else
                UserActionObserver.UnregistTargetObserver(TableRow.actionType, QuestData.actionTargetID, CheckClear);

            // new ApplyReward(GameInstance.MainUser, res.rewardData);
        }

        protected override void SetDescription()
        {
            switch(TableRow.actionType)
                {
                    case EActionType.OwnCrop:
                    description = $"작물을 {TableRow.targetValue}개 획득하시오.";
                        break;
                    case EActionType.AdventureComplete:
                    description = $"탐험을 {TableRow.targetValue}번 성공하시오.";
                        break;
                    case EActionType.HatchEgg:
                    description = $"알을 {TableRow.targetValue}개 부화하시오.";
                        break;
                    case EActionType.PlantSeed:
                    description = $"씨앗을 {TableRow.targetValue}개 심으시오.";
                        break;
                    case EActionType.HarvestCrop:
                    description = $"작물을 {TableRow.targetValue}개 수확하시오.";
                        break;
                }
        }
    }
}