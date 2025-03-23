using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectF.Quests
{
    public class PlayTimeQuest : Quest
    {
        private float targetTime;
        private float currentTime;

        public float TargetTime => targetTime;

        public PlayTimeQuest(float targetTime)
        {
            questType = Datas.EQuestType.PlayTime;
            message = ResourceUtility.GetQusetDescriptionLocalKey(questType);
            //키로 메세지 받아오기 해야함
            message = message.Replace("{targetTime}", $"{targetTime}");
            this.targetTime = targetTime;
        }

        public PlayTimeQuest(QuestSO so)
        {
            questData = so;
            if(questData is PlayTimeQusetSO)
            {
                this.targetTime = ((PlayTimeQusetSO)questData).TargetTime;
                currentTime = 0.0f;
            
                message = $"{targetTime}초 동안 게임을 플레이하기";
            }   
        }

        public override void Update()
        {
            base.Update();

            currentTime += Time.deltaTime;

            UpdateQuest();
        }

        protected override bool CheckQuestClear()
        {
            return currentTime >= targetTime;
        }

        protected override void MakeReward()
        {
            
        }
    }
}
