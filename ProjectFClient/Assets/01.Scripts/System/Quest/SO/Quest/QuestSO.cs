using System.Collections;
using System.Collections.Generic;
using ProjectF.SubCharacters;
using UnityEngine;

namespace ProjectF.Quests
{
    public abstract class QuestSO : ScriptableObject
    {
        [SerializeField][TextArea] private List<string> onMakeTexts;
        [SerializeField][TextArea] private List<string> onClearTexts;

        public List<string> OnMakeTexts => onMakeTexts;
        public List<string> OnClearTexts => onClearTexts;

        public abstract Quest MakeQuest();
    }
}