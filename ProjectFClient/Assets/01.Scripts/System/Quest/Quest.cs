using System;
using System.Collections;
using System.Collections.Generic;
using ProjectF.Datas;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace ProjectF.Quests
{
    public abstract class Quest
    {
        private EQuestType questType;
        public EQuestType QuestType => questType;

        private string questName;
        public string QuestName => questName;

        protected string message;
        public string Message => message;

        private bool canClear = false;
        public bool CanClear => canClear;

        string rewordType1;
        int rewordAmount1;
        string rewordType2;
        int rewordAmount2;
        string rewordType3;
        int rewordAmount3;

        public event Action<Quest> OnMakeQuestEvent;
        public event Action<Quest> OnCanClearQuestEvent;
        public event Action<Quest> OnClearQuestEvent;

        public Quest(QuestData data)
        {
            this.questType = data.questType;
            this.questName = data.questName;
            this.canClear = data.canClear;
            this.rewordType1 = data.rewordType1;
            this.rewordType2 = data.rewordType2;
            this.rewordType3 = data.rewordType3;
            this.rewordAmount1 = data.rewordAmount1;
            this.rewordAmount2 = data.rewordAmount2;
            this.rewordAmount3 = data.rewordAmount3;
            SetMessage();
        }

        public Quest(
            EQuestType questType,
            string questName,
            string rewordType1, 
            int rewordAmount1,
            string rewordType2,
            int rewordAmount2,
            string rewordType3,
            int rewordAmount3,
            params object[] parameters)
        {
            this.questType = questType;
            this.questName = questName;
            this.rewordType1 = rewordType1;
            this.rewordType2 = rewordType2;
            this.rewordType3 = rewordType3;
            this.rewordAmount1 = rewordAmount1;
            this.rewordAmount2 = rewordAmount2;
            this.rewordAmount3 = rewordAmount3;
            SetParameters(parameters);
            SetMessage();
        }

        protected abstract void SetParameters(params object[] parameters);
        protected virtual void SetMessage()
        {
            message = ResourceUtility.GetQusetDescriptionLocalKey(questType);
        }

        protected abstract bool CheckQuestClear();
        private void MakeReward()
        {
            QuestUtility.MakeReword(rewordType1, rewordAmount1);
            QuestUtility.MakeReword(rewordType2, rewordAmount2);
            QuestUtility.MakeReword(rewordType3, rewordAmount3);
        }

        public virtual void OnMakeQuest() 
        {
            OnMakeQuestEvent?.Invoke(this);

            UpdateQuest();
        }

        //매 프레임 실행되는 업데이트
        public virtual void Update() {}

        //퀘스트 정보 갱신으로 사용하는 업데이트
        protected virtual void UpdateQuest()
        {
            if(CheckQuestClear() && !canClear)
            {
                canClear = true;
                Debug.Log($"Can clear quset : {GetType()}");
                OnCanClearQuestEvent?.Invoke(this);
                QuestManager.Instance.ClearQuest(this);
            }
        }

        public virtual void OnClearQuest() 
        {
            MakeReward();

            OnClearQuestEvent?.Invoke(this);
        }
    }
}