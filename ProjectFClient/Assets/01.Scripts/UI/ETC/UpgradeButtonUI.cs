using Cysharp.Threading.Tasks;
using H00N.Extensions;
using H00N.OptOptions;
using H00N.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProjectF.StringUtility;
using static ProjectF.GameDefine;
using System;

namespace ProjectF.UI
{
    public class UpgradeButtonUI : MonoBehaviourUI
    {
        private const float UPDATE_DELAY = 1f;

        [SerializeField] OptOption<AddressableAsset<Sprite>> buttonImageOption = null;
        [SerializeField] Image iconImage = null;
        [SerializeField] Image buttonImage = null;
        [SerializeField] TMP_Text goldText = null;

        private int costItemValue = 0;
        private Func<bool> upgradePossibleFactory = null;

        private bool upgradePossible = false;
        public bool UpgradePossible => upgradePossible;

        public async void Initialize(string costItemIconKey, int costItemValue, Func<bool> upgradePossibleFactory)
        {
            this.costItemValue = costItemValue;
            this.upgradePossibleFactory = upgradePossibleFactory;
            upgradePossible = false;

            new SetSprite(iconImage, costItemIconKey);
            await UniTask.WhenAll(buttonImageOption[true].InitializeAsync(), buttonImageOption[false].InitializeAsync());
            StopAllCoroutines();
            StartCoroutine(this.LoopRoutine(UPDATE_DELAY, RefreshUI, 0f));
        }

        public new void Release()
        {
            base.Release();
            StopAllCoroutines();
        }

        private void RefreshUI()
        {
            upgradePossible = upgradePossibleFactory.Invoke();
            buttonImage.sprite = ResourceManager.GetResource<Sprite>(buttonImageOption[upgradePossible].Key);
            goldText.text = ColorTag(DefaultColorOption[upgradePossible], costItemValue);
        }
    }
}
