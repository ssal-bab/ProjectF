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

        private EActionType actionType;

        private int targetID;

        public RepeatQuest(RepeatQuestTableRow tableRow, ERepeatQuestType repeatQuestType) : base(tableRow)
        {
            this.repeatQuestType = repeatQuestType;
            this.actionType = tableRow.actionType;
            targetID = -1;
            SetDescription();
        }

        public RepeatQuest(RepeatQuestTableRow tableRow, ERepeatQuestType repeatQuestType, int targetID) : base(tableRow)
        {
            this.repeatQuestType = repeatQuestType;
            this.targetID = targetID;
            this.actionType = tableRow.actionType;
            SetDescription();
        }

        public override void StartQuest()
        {
            base.StartQuest();

            if(targetID == -1)
            {
                UserActionObserver.RegistObserver(actionType, CheckClear);
            }
            else
            {
                UserActionObserver.RegistTargetObserver(actionType, targetID, CheckClear);
            }
        }

        private async void CheckClear()
        {
            ClearRepeatQuestRequest req = new ClearRepeatQuestRequest(repeatQuestType);
            ClearRepeatQuestResponse res = await NetworkManager.Instance.SendWebRequestAsync<ClearRepeatQuestResponse>(req);
            if(res.result != ENetworkResult.Success)
            {
                return;
            }
                
            Debug.Log($"clear repeat quest : {repeatQuestType}");

            RepeatQuestTableRow tableRow = this.TableRow as RepeatQuestTableRow;
            if(targetID == -1)
                UserActionObserver.UnregistObserver(tableRow.actionType, CheckClear);
            else
                UserActionObserver.UnregistTargetObserver(tableRow.actionType, targetID, CheckClear);

            new ApplyReward(GameInstance.MainUser, res.rewardData);

            OnCanClear();
        }

        protected override void SetDescription()
        {
            switch(actionType)
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