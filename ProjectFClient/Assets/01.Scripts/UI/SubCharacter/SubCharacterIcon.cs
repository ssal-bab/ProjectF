using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.SubCharacter
{
    public class SubCharacterIcon : MonoBehaviourUI
    {
        [SerializeField] private TextMeshProUGUI messageText;
        
        [SerializeField] private Button button;
        public Button Button => button;

        public void SetMessage(string message)
        {
            messageText.SetText(message);
        }
    }
}