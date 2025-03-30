using System.Collections;
using System.Collections.Generic;
using ProjectF.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF
{
    public class QuestButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] Image notify;

        [SerializeField] Sprite makeIcon;
        [SerializeField] Sprite clearIcon;

        void Start()
        {
            button.onClick.AddListener(StartQuest);
            QuestManager.Instance.OnAddWaitingQuest += OnAddWaitingQuest;
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(StartQuest);
        }

        private void OnAddWaitingQuest(Quest quest)
        {
            notify.sprite = makeIcon;
        }

        private void StartQuest()
        {
            if(QuestManager.Instance.waitingQuestCount <= 0)
                return;

            DialogueManager.Instance.StartDialogue(Dialogues.ESpeakerType.Admin,
             $"[{QuestManager.Instance.waitingQuests.Peek().Description}]", OnStartQuest);

        }

        private void OnStartQuest()
        {
            QuestManager.Instance.StartQuestInWaitingQuest();
            if(QuestManager.Instance.waitingQuestCount <= 0)
                notify.gameObject.SetActive(false);
        }

    }
}
