using H00N.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProjectF.StringUtility;
using static ProjectF.GameDefine;

namespace ProjectF.UI
{
    public class MaterialOptionUI : MonoBehaviourUI
    {
        private const float UPDATE_DELAY = 1f;

        [SerializeField] Image iconImage = null;
        [SerializeField] TMP_Text countText = null;
        [SerializeField] GameObject checkObject = null;
        private int materialID = 0;
        private int targetCount = 0;

        private bool optionChecked = false;
        public bool OptionChecked => optionChecked;

        public void Initialize(int materialID, int targetCount)
        {
            base.Initialize();

            this.materialID = materialID;
            this.targetCount = targetCount;
            iconImage.sprite = ResourceUtility.GetMaterialIcon(materialID);

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
            if(GameInstance.MainUser.storageData.materialStorage.TryGetValue(materialID, out int count) == false)
                return;

            optionChecked = count >= targetCount;
            if(checkObject.activeSelf != optionChecked)
                checkObject.SetActive(optionChecked);

            countText.text = $"{ColorTag(DefaultColorOption[optionChecked], count)}/{targetCount}";
        }
    }
}
