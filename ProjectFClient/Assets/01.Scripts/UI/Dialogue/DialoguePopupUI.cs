using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectF.Quests;
using TMPro;
using UnityEngine.UI;
using System;
using ProjectF.SubCharacters;

namespace ProjectF.UI.Quests
{
    public class DialoguePopupUI : MonoBehaviourUI
    {
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button button;
        [SerializeField] private Image speakerImage; 

        [SerializeField] private List<string> currentDialogueTexts;
        private int currentDialogueTextIndex;

        public event Action OnEndDialogue;
        private Action additiveEndDialogueAction;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);

            button.onClick.AddListener(ShowNextText);

            //test
            SubCharacterManager.Instance.dialogueUI = this;
        }

        public void Show(List<string> showTexts, Action onEndAction)
        {
            currentDialogueTexts = showTexts;
            additiveEndDialogueAction = onEndAction;
            OnEndDialogue += additiveEndDialogueAction;

            ShowTextFromStart();

            gameObject.SetActive(true);
        }

        private void ShowTextFromStart()
        {
            currentDialogueTextIndex = -1;
            ShowNextText();
        }

        private void ShowNextText()
        {
            currentDialogueTextIndex++;

            if(currentDialogueTexts.Count <= currentDialogueTextIndex)
            {
                EndDialogue();

                return;
            }

            ShowText(currentDialogueTexts[currentDialogueTextIndex]);
        }

        private void ShowText(string text)
        {
            dialogueText.SetText(text);
        }

        public void SetSpeakerImage(Sprite image)
        {
            speakerImage.sprite = image;
        }

        public void EndDialogue()
        {
            OnEndDialogue?.Invoke();
            OnEndDialogue -= additiveEndDialogueAction;
            additiveEndDialogueAction = null;

            gameObject.SetActive(false);
        }
    }
}