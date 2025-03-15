using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectF.Quests;
using ProjectF.UI.SubCharacter;
using H00N.Resources;

namespace ProjectF.SubCharacters
{
    public class SubCharacter
    {
        private ESubCharacterType characterType;

        private Queue<Quest> qusetQueue = null;
        private Quest currentQuest = null;

        private SubCharacterSO data;
        public SubCharacterSO Data => data;

        private SubCharacterIcon icon = null;

        public SubCharacter(SubCharacterSO data)
        {
            qusetQueue = new();
            this.data = data;

            Debug.Log($"created sub character : {data.CharacterType}");
        }

        public void SetIcon(SubCharacterIcon icon)
        {
            this.icon = icon;
            icon.Button.onClick.AddListener(MakeQuest);

            //test
            //EnqueueQuest(new PlayTimeQuest(10.0f));
            
            if(data.QusetContainerData)
                EnqueueQuest(data.QusetContainerData.QuestDataList[0]);

        }

        public void EnqueueQuest(QuestSO questData)
        {
            if(!questData)
                return;

            EnqueueQuest(questData.MakeQuest());
        }

        public void EnqueueQuest(Quest newQuest)
        {
            if(qusetQueue.Contains(newQuest))
                return;

            qusetQueue.Enqueue(newQuest);

            if(currentQuest == null)
            {
                icon.SetMessage("!");
            }
        }

        private void MakeQuest()
        {
            if(qusetQueue.Count < 1)
                return;
            if(currentQuest != null)
                return;
            
            Quest quest = qusetQueue.Dequeue();
            currentQuest = quest;
            currentQuest.OnCanClearQuestEvent += OnCanClearQuest;

            icon.SetMessage("");

            icon.Button.onClick.RemoveListener(MakeQuest);
            icon.Button.onClick.AddListener(ClearQuset);

            SubCharacterManager.Instance.StartDialogue(Data.CharacterType, quest.QuestData.OnMakeTexts, () => {
                QuestManager.Instance.MakeQuest(quest);
            });
        }

        private void OnCanClearQuest(Quest quest)
        {
            icon.SetMessage("?");
        }

        public void ClearQuset()
        {
            if(currentQuest == null)
                return;
            if(!currentQuest.CanClear)
                return;

            icon.SetMessage(qusetQueue.Count > 0 ? "!" : "");

            SubCharacterManager.Instance.StartDialogue(Data.CharacterType, currentQuest.QuestData.OnClearTexts, () => {
                QuestManager.Instance.ClearQuest(currentQuest);
                currentQuest = null;
            });

            icon.Button.onClick.RemoveListener(ClearQuset);
            icon.Button.onClick.AddListener(MakeQuest);
        }
    }
}