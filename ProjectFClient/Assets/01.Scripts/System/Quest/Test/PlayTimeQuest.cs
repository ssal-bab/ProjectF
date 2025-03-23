using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectF.Datas;
using DocumentFormat.OpenXml.Wordprocessing;
using System;

namespace ProjectF.Quests
{
    public class PlayTimeQuest : Quest
    {
        private float targetTime;
        private float currentTime;

        public float TargetTime => targetTime;

        public PlayTimeQuest(EQuestType questType, string questName, params object[] parameters) : base(questType, questName, parameters)
        {
            currentTime = 0.0f;
        }

        // public PlayTimeQuest(QuestSO so)
        // {
        //     questData = so;
        //     if(questData is PlayTimeQusetSO)
        //     {
        //         this.targetTime = ((PlayTimeQusetSO)questData).TargetTime;
        //         currentTime = 0.0f;
            
        //         message = $"{targetTime}초 동안 게임을 플레이하기";
        //     }   
        // }

        protected override void SetParameters(params object[] parameters)
        {
            targetTime = (float)parameters[0];
        }

        protected override void SetMessage()
        {
            base.SetMessage();

            message = message.Replace("{targetTime}", $"{targetTime}");
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
