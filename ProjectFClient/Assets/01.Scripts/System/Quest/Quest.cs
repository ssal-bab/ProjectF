using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace ProjectF.Quests
{
    public abstract class Quest
    {
        protected string message;
        public string Message => message;

        private bool canClear = false;
        public bool CanClear => canClear;

        protected abstract bool CheckQuestClear();
        protected abstract void MakeReward();

        public virtual void OnMakeQuest() 
        {
            UpdateQuest();
        }

        //매 프레임 실행되는 업데이트
        public virtual void Update() {}

        //퀘스트 정보 갱신으로 사용하는 업데이트
        protected virtual void UpdateQuest()
        {
            if(CheckQuestClear())
            {
                canClear = true;

                //테스트
                QuestManager.Instance.ClearQuest(this);
            }
        }

        public virtual void OnClearQuest() 
        {
            MakeReward();
        }
    }
}