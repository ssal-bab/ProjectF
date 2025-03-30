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

            //MakeQuest(DataTableManager.GetTable<QuestTable>()[0]);
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
            quests?.Clear();
            quests = null;
            OnMakeQuest = null;
            OnClearQuest = null;

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

            //make quest packet
            // MakeQuestRequest req = new MakeQuestRequest(newQuest.MakeQusetData());
            // MakeQuestResponse res = await NetworkManager.Instance.SendWebRequestAsync<MakeQuestResponse>(req);
            // if(res.result != ENetworkResult.Success)
            // {
            //     Debug.Log(res.result);
            //     return;
            // }

            quests.Add(newQuest);
            newQuest.OnMakeQuest();
            OnMakeQuest?.Invoke(newQuest);

            Debug.Log($"Make Quest : {newQuest.QuestName}");
        }

        public void ClearQuest(Quest clearedQuest)
        {
            //clear quest packet
            // ClearQuestRequest req = new ClearQuestRequest(clearedQuest.MakeQusetData());
            // ClearQuestResponse res = await NetworkManager.Instance.SendWebRequestAsync<ClearQuestResponse>(req);
            // if(res.result != ENetworkResult.Success)
            //     return;

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