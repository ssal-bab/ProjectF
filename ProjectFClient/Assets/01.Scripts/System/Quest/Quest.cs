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

        public virtual void OnMakeQuest() 
        {
            UpdateQuest();
        }

        protected abstract bool CheckQuestClear();
        protected abstract void MakeReward();

        protected virtual void UpdateQuest()
        {
            if(CheckQuestClear())
            {
                canClear = true;
            }
        }

        public virtual void OnClearQuest() 
        {
            MakeReward();
        }
    }
}