using H00N.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProjectF.StringUtility;
using static ProjectF.GameDefine;

namespace ProjectF.UI
{
    public class CostOptionUI : MonoBehaviourUI
    {
        private const float UPDATE_DELAY = 1f;

        [SerializeField] Image iconImage = null;
        [SerializeField] TMP_Text countText = null;
        [SerializeField] GameObject checkObject = null;
        private int cropID = 0;
        private int targetCount = 0;

        private bool optionChecked = false;
        public bool OptionChecked => optionChecked;

        public void Initialize(int cropID, int targetCount)
        {
            base.Initialize();

            this.cropID = cropID;
            this.targetCount = targetCount;
            new SetSprite(iconImage, ResourceUtility.GetCropIconKey(cropID));

            optionChecked = false;
            StartCoroutine(this.LoopRoutine(UPDATE_DELAY, RefreshUI, 0f));
        }

        public new void Release()
        {
            base.Release();
            StopAllCoroutines();
        }

        private void RefreshUI()
        {
            if(GameInstance.MainUser.storageData.cropStorage.TryGetValue(cropID, out var category) == false)
                return;

            int count = 0;
            category.Values.ForEach(i => count += i);

            optionChecked = count >= targetCount;
            if(checkObject.activeSelf != optionChecked)
                checkObject.SetActive(optionChecked);

            countText.text = $"{ColorTag(DefaultColorOption[optionChecked], count)}/{targetCount}";
        }
    }
}
