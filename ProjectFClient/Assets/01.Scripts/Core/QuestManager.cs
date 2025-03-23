using System;
using System.Collections.Generic;
using H00N.DataTables;
using Newtonsoft.Json;
using ProjectF.Datas;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.Quests
{
    public class QuestManager
    {
        public event Action<Quest> OnMakeQuest;
        public event Action<Quest> OnClearQuest;

        private List<Quest> quests;
        public List<Quest> Quests => quests;

        private static QuestManager instance = null;
        public static QuestManager Instance => instance;

        public void Initialize()
        {
            instance = this;

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
                        quests.Add(Convert.ChangeType(JsonConvert.DeserializeObject(value), pair.Key) as Quest);
                    }
                }
            }

            MakeQuest(DataTableManager.GetTable<QuestTable>()[0]);
        }

        public void Update()
        {
            for(int i = quests.Count - 1; i >= 0; i--)
            {
                quests[i].Update();
            }
        }

        public void Release()
        {
            GameInstance.MainUser.questData.quests.Clear();
            foreach(Quest quest in quests)
            {
                if(!GameInstance.MainUser.questData.quests.ContainsKey(quest.GetType()))
                {
                    GameInstance.MainUser.questData.quests.Add(quest.GetType(), new());
                }

                GameInstance.MainUser.questData.quests[quest.GetType()].Add(JsonConvert.SerializeObject(quest));
            }

            quests?.Clear();
            quests = null;
            OnMakeQuest = null;
            OnClearQuest = null;

            instance = null;
        }

        public void MakeQuest(QuestTableRow questTableRow)
        {
            MakeQuest(QuestUtility.CreateQuest(questTableRow));
        }

        public void MakeQuest(Quest newQuest)
        {
            quests.Add(newQuest);
            newQuest.OnMakeQuest();
            OnMakeQuest?.Invoke(newQuest);

            Debug.Log($"Make Quest : {newQuest.QuestName}");
        }

        public void ClearQuest(Quest clearedQuest)
        {
            if(!quests.Contains(clearedQuest))
                return;
            if(!clearedQuest.CanClear)
                return;

            clearedQuest.OnClearQuest();
            quests.Remove(clearedQuest);
            OnClearQuest?.Invoke(clearedQuest);   

            Debug.Log($"Clear Quest : {clearedQuest}");
        }
    }
}