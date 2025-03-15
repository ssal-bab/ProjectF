using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectF.Quests
{
    [CreateAssetMenu(menuName = "SO/Quset/QusetContainer")]
    public class QusetContainerSO : ScriptableObject
    {
        [SerializeField] private List<QuestSO> questDataList;
        public List<QuestSO> QuestDataList => questDataList;
    }
}
