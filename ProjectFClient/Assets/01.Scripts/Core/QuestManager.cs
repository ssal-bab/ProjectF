using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ProjectF.Datas;
using ProjectF.Quests;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

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
            quests = new();
            OnMakeQuest = null;
            OnClearQuest = null;

            UserQuestData questData = GameInstance.MainUser.questData;
            if(questData != null)
            {
                foreach(var pair in questData.quests)
                {
                    foreach(var value in pair.Value)
                    {
                        QuestManager.quests.Add(Convert.ChangeType(JsonConvert.DeserializeObject(value), pair.Key) as Quest);
                    }
                }

                // foreach(var quest in quests)
                // {
                //     Debug.Log(quest);
                // }
            }
        }

        public static void Release()
        {
            quests?.Clear();
            quests = null;
            OnMakeQuest = null;
            OnClearQuest = null;
        }

        public static void MakeQuest(Quest newQuest)
        {
            quests.Add(newQuest);
            newQuest.OnMakeQuest();
            OnMakeQuest?.Invoke(newQuest);
            Debug.Log(newQuest);
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