using System;
using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.Quests
{
    public abstract class Quest
    {
        private int id;
        public int Id => id;

        private string questName;
        public string QuestName => questName;

        protected string message;
        public string Message => message;

        private bool canClear = false;
        public bool CanClear => canClear;

        public event Action<Quest> OnMakeQuestEvent;
        public event Action<Quest> OnCanClearQuestEvent;
        public event Action<Quest> OnClearQuestEvent;

        public Quest()
        {
            SetMessage();
        }

        protected virtual void SetMessage()
        {
            //message = ResourceUtility.GetQusetDescriptionLocalKey(questType);
        }

        protected abstract bool CheckQuestClear();
        private void MakeReward()
        {

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