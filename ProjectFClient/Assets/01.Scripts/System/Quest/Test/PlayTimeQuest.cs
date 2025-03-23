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

        public PlayTimeQuest(
            EQuestType questType,
            string questName, 
            string rewordType1, 
            int rewordAmount1,
            string rewordType2,
            int rewordAmount2,
            string rewordType3,
            int rewordAmount3,
            params object[] parameters) : 
            base(questType, 
            questName, 
            rewordType1, 
            rewordAmount1,
            rewordType2,
            rewordAmount2,
            rewordType3,
            rewordAmount3,
            parameters)
        {
            currentTime = 0.0f;
        }

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
    }
}
