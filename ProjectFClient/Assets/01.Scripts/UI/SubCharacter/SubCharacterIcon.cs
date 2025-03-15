using System.Collections;
using System.Collections.Generic;
using ProjectF.SubCharacters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.SubCharacters
{
    public class SubCharacterIcon : MonoBehaviourUI
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Image characterImage;

        [SerializeField] private Button button;
        public Button Button => button;

        public void SetCharacter(SubCharacter character)
        {
            characterImage.sprite = character.Data.Image;
        }

        public void SetMessage(string message)
        {
            messageText.SetText(message);
        }
    }
}