using System.Collections;
using System.Collections.Generic;
using ProjectF.Quests;
using UnityEngine;

namespace ProjectF.SubCharacters
{
    [CreateAssetMenu(menuName = "SO/SubCharacter")]
    public class SubCharacterSO : ScriptableObject
    {
        public ESubCharacterType CharacterType;
        public Sprite Image;
        public QusetContainerSO QusetContainerData;
    }
}
