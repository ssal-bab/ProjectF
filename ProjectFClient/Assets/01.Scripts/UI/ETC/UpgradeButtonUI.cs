using Cysharp.Threading.Tasks;
using H00N.Extensions;
using H00N.OptOptions;
using H00N.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI
{
    public class UpgradeButtonUI : MonoBehaviourUI
    {
        private const float UPDATE_DELAY = 1f;

        [SerializeField] OptOption<AddressableAsset<Sprite>> buttonImageOption = null;
        [SerializeField] Image buttonImage = null;
        [SerializeField] TMP_Text goldText = null;

        private int targetGold = 0;

        public async void Initialize(int targetGold)
        {
            this.targetGold = targetGold;

            await UniTask.WhenAll(buttonImageOption[true].InitializeAsync(), buttonImageOption[false].InitializeAsync());
            StartCoroutine(this.LoopRoutine(UPDATE_DELAY, RefreshUI, 0f));
        }

        public new void Release()
        {
            base.Release();
            StopAllCoroutines();
        }

        private void RefreshUI()
        {
            bool optionChecked = GameInstance.MainUser.monetaData.gold >= targetGold;
            
            buttonImage.sprite = ResourceManager.LoadResource<Sprite>(buttonImageOption[optionChecked].Key);
            goldText.text = $"<Color={GameDefine.DefaultColorOption[optionChecked]}>{targetGold}</Color>";
        }
    }
}
