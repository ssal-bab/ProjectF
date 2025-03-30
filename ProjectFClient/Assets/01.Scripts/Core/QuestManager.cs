using System;
using System.Collections.Generic;
using H00N.DataTables;
using Newtonsoft.Json;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.Packets;
using ProjectF.Networks;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.PlayerLoop;

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

        private RepeatQuestController repeatQuestController;

        public Queue<Quest> waitingQuests;
        public Action<Quest> OnAddWaitingQuest;
        public int waitingQuestCount => waitingQuests.Count;


        public void AddWaitingQuest(Quest quest)
        {
            if(quest == null)
                return;

            waitingQuests.Enqueue(quest);
            OnAddWaitingQuest?.Invoke(quest);
        }

        public void StartQuestInWaitingQuest()
        {
            waitingQuests.Dequeue().StartQuest();
        }

        public void Initialize()
        {
            instance = this;

            quests = new();
            waitingQuests = new();
            OnMakeQuest = null;
            OnClearQuest = null;

            repeatQuestController = new RepeatQuestController();
            repeatQuestController.Initialize();
        }

        public void Release()
        {
            quests?.Clear();
            quests = null;
            OnMakeQuest = null;
            OnClearQuest = null;

            repeatQuestController.Release();

            instance = null;
        }

        public void MakeQuest(QuestTableRow questTableRow)
        {
            //MakeQuest(QuestUtility.CreateQuest(questTableRow));
        }

        public void MakeQuest(Quest newQuest)
        {
            if(newQuest == null)
                return;
                
            quests.Add(newQuest);
            newQuest.OnMakeQuest();
            OnMakeQuest?.Invoke(newQuest);

            Debug.Log($"Make Quest :");
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