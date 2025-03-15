using System.Collections;
using System.Collections.Generic;
using ProjectF.SubCharacters;
using UnityEngine;

namespace ProjectF.UI.SubCharacter
{
    public class SubCharacterPanel : MonoBehaviourUI
    {
        [SerializeField] private SubCharacterIcon subCharacterIconPrefab;

        protected override void Awake()
        {
            base.Awake();

            foreach(var pair in SubCharacterManager.Instance.SubCharacters)
            {
                SubCharacterIcon subCharacterIcon = Instantiate(subCharacterIconPrefab);
                subCharacterIcon.name = $"{pair.Key}Icon";
                pair.Value.SetIcon(subCharacterIcon);
                subCharacterIcon.transform.SetParent(transform);
            }
        }
    }
}