using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectF.UI.Quests;
using H00N.Resources;

namespace ProjectF.SubCharacters
{
    public class SubCharacterManager
    {
        private Dictionary<ESubCharacterType, SubCharacter> subCharacters = null;
        public Dictionary<ESubCharacterType, SubCharacter> SubCharacters => subCharacters;

        private static SubCharacterManager instance;
        public static SubCharacterManager Instance => instance;

        public DialoguePopupUI dialogueUI;

        public void Initialize()
        {
            instance = this;

            subCharacters = new();
            foreach(ESubCharacterType type in Enum.GetValues(typeof(ESubCharacterType)))
            {
                SubCharacter subChar = new SubCharacter(ResourceManager.LoadResource<SubCharacterSO>($"SubCharacterData_{type}"));
                subCharacters.Add(type, subChar);
            }
        }

        public void Release()
        {

        }

        public void StartDialogue(ESubCharacterType characterType, List<string> dialogueTexts, Action onEndAction)
        {
            //dialogueUI.SetSpeakerImage(subCharacters[characterType].Data.Image);
            //dialogueUI.Show(dialogueTexts, onEndAction);
        }
    }
}