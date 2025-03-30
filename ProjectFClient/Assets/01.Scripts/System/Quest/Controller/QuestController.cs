using System.Collections;
using System.Collections.Generic;
using ProjectF.Quests;
using UnityEngine;

namespace ProjectF
{
    public abstract class QuestController
    {
        public abstract void Initialize();
        public abstract void Release();

        public virtual void MakeQuest(Quest quest, bool startByUser, bool clearByUser)
        {
            if(startByUser)
            {
                QuestManager.Instance.AddWaitingQuest(quest);
            }
            else
            {
                quest.StartQuest();
            }
        }
    }
}
