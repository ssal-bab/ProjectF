using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using ProjectF.Dialogues;
using H00N.Resources.Pools;

namespace ProjectF.UI.Quests
{
    public class DialoguePopupUI : PoolableBehaviourUI
    {
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button button;
        [SerializeField] private Image speakerImage; 
        [SerializeField] private TextMeshProUGUI speakerNameText; 

        [SerializeField] private List<string> currentDialogueTexts;
        private int currentDialogueTextIndex;

        private bool isTexting;

        public event Action OnEndDialogue;
        private Action additiveEndDialogueAction;

        public new void Initialize()
        {
            base.Initialize();

            button.onClick.AddListener(ShowNextText);
        }

        public void Show(ESpeakerType speakerType, List<string> showTexts, Action onEndAction)
        {
            SetSpeaker(speakerType);
            currentDialogueTexts = showTexts;
            additiveEndDialogueAction = onEndAction;
            OnEndDialogue += additiveEndDialogueAction;

            ShowTextFromStart();
        }

        private void ShowTextFromStart()
        {
            currentDialogueTextIndex = 0;
            isTexting = false;
            ShowText(currentDialogueTexts[currentDialogueTextIndex]);
        }

        private void ShowNextText()
        {
            if(dialogueText.text.Length == currentDialogueTexts[currentDialogueTextIndex].Length)
            {
                currentDialogueTextIndex++;
                isTexting = false;

                if(currentDialogueTexts.Count <= currentDialogueTextIndex)
                {
                    EndDialogue();

                    return;
                }
            }

            ShowText(currentDialogueTexts[currentDialogueTextIndex]);
        }

        private void ShowText(string text)
        {
            if(!isTexting)
            {
                TMPExtension.DOText(dialogueText, text, 0.05f);
            }
            else
            {
                dialogueText.StopAllCoroutines();
                dialogueText.SetText(text);
            }

            isTexting = !isTexting;
        }

        public void SetSpeaker(ESpeakerType speakerType)
        {
            //speakerNameText.SetText(ResourceUtility.GetDialogueSpeakerNameLocakKey(speakerType));
            speakerNameText.SetText(speakerType.ToString());
            new SetSprite(speakerImage, ResourceUtility.GetDialogueSpeakerImageKey((int)speakerType));
            speakerImage.SetNativeSize();
            float ratio = GameDefine.DialogueSpeakerImageSizeWidth / speakerImage.rectTransform.sizeDelta.x;
            speakerImage.rectTransform.sizeDelta = new Vector2(
                GameDefine.DialogueSpeakerImageSizeWidth, speakerImage.rectTransform.sizeDelta.y * ratio);
        }

        public void EndDialogue()
        {
            OnEndDialogue?.Invoke();
            OnEndDialogue -= additiveEndDialogueAction;
            additiveEndDialogueAction = null;

            Release();
            PoolManager.Despawn(this);
        }

        protected override void Release()
        {
            base.Release();

            button.onClick.RemoveListener(ShowNextText);
        }
    }
}