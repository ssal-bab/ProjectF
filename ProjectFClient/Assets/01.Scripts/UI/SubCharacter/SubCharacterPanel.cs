using System.Collections;
using System.Collections.Generic;
using ProjectF.SubCharacters;
using UnityEngine;

namespace ProjectF.UI.SubCharacters
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
                subCharacterIcon.SetCharacter(pair.Value);
                pair.Value.SetIcon(subCharacterIcon);
                subCharacterIcon.transform.SetParent(transform);
                subCharacterIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }
}