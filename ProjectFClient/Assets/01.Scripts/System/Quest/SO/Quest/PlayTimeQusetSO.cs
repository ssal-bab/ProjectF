using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectF.SubCharacters;

namespace ProjectF.Quests
{
    [CreateAssetMenu(menuName = "SO/Quset/Quset/PlayTime")]
    public class PlayTimeQusetSO : QuestSO
    {
        [SerializeField] private float targetTime;
        public float TargetTime => targetTime;

        public override Quest MakeQuest()
        {
            PlayTimeQuest quest = new PlayTimeQuest(this);

            return quest;
        }
    }
}
