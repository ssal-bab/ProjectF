using System;
using H00N.Resources.Pools;
using TMPro;
using UnityEngine;

namespace ProjectF.UI.Cheats
{
    public class CheatInputPopupUI : PoolableBehaviourUI
    {
        [SerializeField] TMP_Text descriptionText = null;
        [SerializeField] TMP_InputField inputFieldFirstParam = null;
        [SerializeField] TMP_InputField inputFieldSecondParam = null;

        private Action<string> onConfirmCallbackOneParam = null;
        private Action<string, string> onConfirmCallbackTwoParam = null;

        public void Initialize(string description, Action<string> onConfirmCallback)
        {
            onConfirmCallbackOneParam = onConfirmCallback;
            descriptionText.text = description;
            inputFieldFirstParam.text = "";
            inputFieldSecondParam.gameObject.SetActive(false);
        }

        public void Initialize(string description, Action<string, string> onConfirmCallback)
        {
            onConfirmCallbackTwoParam = onConfirmCallback;
            descriptionText.text = description;
            inputFieldFirstParam.text = "";
            inputFieldSecondParam.text = "";
            inputFieldSecondParam.gameObject.SetActive(true);
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
        }

        public void OnTouchConfirmButton()
        {
            if(inputFieldSecondParam.gameObject.activeSelf)
            {
                onConfirmCallbackTwoParam?.Invoke(inputFieldFirstParam.text, inputFieldSecondParam.text);
            }
            else
            {
                onConfirmCallbackOneParam?.Invoke(inputFieldFirstParam.text);
            }
        }
    }
}