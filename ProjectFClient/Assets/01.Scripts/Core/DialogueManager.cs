using System.Collections;
using System.Collections.Generic;
using H00N.Resources.Pools;
using ProjectF.UI.Quests;
using UnityEngine;
using System;
using ProjectF.Dialogues;
using H00N.Resources;

namespace ProjectF
{
    public class DialogueManager
    {
        private static DialogueManager instance;
        public static DialogueManager Instance => instance;

        public void Initialize()
        {
            instance = this;
        }

        public void StartDialogue(ESpeakerType speakerType, string dialogueTexts, Action onEndAction)
        {
            StartDialogue(speakerType, DialogueParse(dialogueTexts), onEndAction);
        }

        public async void StartDialogue(ESpeakerType speakerType, List<string> dialogueTexts, Action onEndAction)
        {
            await ResourceManager.LoadResourceAsync("DialoguePopupUI");
            DialoguePopupUI dialogueUI = PoolManager.Spawn<DialoguePopupUI>("DialoguePopupUI", GameDefine.MainPopupFrame);
            dialogueUI.StretchRect();
            dialogueUI.Initialize();
            dialogueUI.Show(speakerType, dialogueTexts, onEndAction);
        }

        public void Release()
        {
            instance = null;
        }

        public List<string> DialogueParse(string input)
        {
            var results = new List<string>();
            int index = 0;

            while (index < input.Length)
            {
                if (input[index] == '[')
                {
                    int textStart = index + 1;
                    int textEnd = input.IndexOf(']', textStart);
                    if (textEnd == -1) break;

                    string text = input.Substring(textStart, textEnd - textStart);

                    results.Add(text.Trim());

                    index = textEnd + 1;

                    // 쉼표 + 공백 스킵
                    while (index < input.Length && (input[index] == ',' || input[index] == ' '))
                        index++;
                }
                else
                {
                    index++;
                }
            }

            return results;
        }
    }
}
