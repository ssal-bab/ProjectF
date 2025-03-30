using System;
using ProjectF.Datas;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.Quests
{
    public abstract class Quest
    { 
        private QuestTableRow tableRow;
        public QuestTableRow TableRow => tableRow;

        protected bool canClear = false;
        public bool CanClear => canClear;

        protected string description;
        public string Description => description;

        public event Action<Quest> OnMakeQuestEvent;
        public event Action<Quest> OnCanClearQuestEvent;
        public event Action<Quest> OnClearQuestEvent;

        public Quest(QuestTableRow tableRow)
        {
            this.tableRow = tableRow;
        }

        public virtual void StartQuest()
        {
            
        }

        protected virtual bool CheckQuestClear()
        {
            return canClear;
        }

        public virtual void OnMakeQuest() 
        {
            OnMakeQuestEvent?.Invoke(this);

            //UpdateQuest();
        }

        //퀘스트 정보 갱신으로 사용하는 업데이트
        protected virtual void UpdateQuest()
        {
            if(canClear)
            {
                Debug.Log($"Can clear quset : {GetType()}");
                OnCanClearQuestEvent?.Invoke(this);
                QuestManager.Instance.ClearQuest(this);
            }
        }

        protected void OnCanClear()
        {
            canClear = true;
            Debug.Log($"Can clear quset : {GetType()}");
            OnCanClearQuestEvent?.Invoke(this);
            QuestManager.Instance.ClearQuest(this);
        }

        public virtual void OnClearQuest() 
        {
            OnClearQuestEvent?.Invoke(this);
        }

        protected abstract void SetDescription();

    }
}