using System;
using H00N.Resources.Pools;
using TMPro;
using UnityEngine;

namespace ProjectF.UI.Cheats
{
    public class CheatInputPopupUI : PoolableBehaviourUI
    {
        [SerializeField] TMP_Text descriptionText = null;
        [SerializeField] TMP_InputField inputField = null;

        private Action<string> onConfirmCallback = null;

        public void Initialize(string description, Action<string> onConfirmCallback)
        {
            this.onConfirmCallback = onConfirmCallback;
            descriptionText.text = description;
            inputField.text = "";
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
        }

        public void OnTouchConfirmButton()
        {
            onConfirmCallback?.Invoke(inputField.text);
        }
    }
}