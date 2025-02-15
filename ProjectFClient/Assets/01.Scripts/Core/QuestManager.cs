using System;
using System.Collections.Generic;
using ProjectF.Quests;
using UnityEngine;

namespace ProjectF
{
    public static class QuestManager
    {
        public static event Action<Quest> OnMakeQuest;
        public static event Action<Quest> OnClearQuest;

        private static List<Quest> quests;
        public static List<Quest> Quests => quests;

        public static void Initialize()
        {
            quests = new List<Quest>();
            OnMakeQuest = null;
            OnClearQuest = null;
        }

        public static void Release()
        {
            quests.Clear();
            OnMakeQuest = null;
            OnClearQuest = null;
        }

        public static void MakeQuest(Quest newQuest)
        {
            quests.Add(newQuest);
            newQuest.OnMakeQuest();
            OnMakeQuest?.Invoke(newQuest);
        }

        public static void ClearQuest(Quest clearedQuest)
        {
            if(!quests.Contains(clearedQuest))
                return;
            if(!clearedQuest.CanClear)
                return;

            clearedQuest.OnClearQuest();
            quests.Remove(clearedQuest);
            OnClearQuest?.Invoke(clearedQuest);   
        }
    }
}