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

        private QuestData questData;
        public QuestData QuestData => questData;

        protected string description;
        public string Description => description;

        public event Action<Quest> OnMakeQuestEvent;
        public event Action<Quest> OnCanClearQuestEvent;
        public event Action<Quest> OnClearQuestEvent;

        public Quest(QuestTableRow tableRow, QuestData questData)
        {
            this.tableRow = tableRow;
            this.questData = questData;
        }

        public virtual void StartQuest()
        {
            if(!TableRow.actionType.ToString().Contains("Target"))
            {
                UserActionObserver.RegistObserver(TableRow.actionType, CheckClear);
            }
            else
            {
                UserActionObserver.RegistTargetObserver(TableRow.actionType, questData.actionTargetID, CheckClear);
            }

            CheckClear();
        }

        protected abstract void CheckClear();
        
        public virtual void OnMakeQuest() 
        {
            OnMakeQuestEvent?.Invoke(this);

            //UpdateQuest();
        }

        //퀘스트 정보 갱신으로 사용하는 업데이트
        protected virtual void UpdateQuest()
        {
            // if(canClear)
            // {
            //     Debug.Log($"Can clear quset : {GetType()}");
            //     OnCanClearQuestEvent?.Invoke(this);
            //     QuestManager.Instance.ClearQuest(this);
            // }
        }

        protected abstract void SetDescription();

    }
}