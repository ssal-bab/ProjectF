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
        protected EQuestType questType;
        public EQuestType QuestType => questType;

        protected string message;
        public string Message => message;

        private bool canClear = false;
        public bool CanClear => canClear;

        protected QuestSO questData;
        public QuestSO QuestData => questData;

        public event Action<Quest> OnMakeQuestEvent;
        public event Action<Quest> OnCanClearQuestEvent;
        public event Action<Quest> OnClearQuestEvent;

        protected abstract bool CheckQuestClear();
        protected abstract void MakeReward();

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
            }
        }

        public virtual void OnClearQuest() 
        {
            MakeReward();

            OnClearQuestEvent?.Invoke(this);
        }
    }
}