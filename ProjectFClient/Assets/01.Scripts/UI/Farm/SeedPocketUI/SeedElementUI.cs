using System;
using H00N.Extensions;
using ProjectF.Datas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class SeedElementUI : PoolableBehaviourUI
    {
        [SerializeField] TMP_Text cropNameText = null;
        [SerializeField] TMP_Text cropPriceText = null;
        [SerializeField] TMP_Text ownCountText = null;
        [SerializeField] Image iconImage = null;
        private int cropID = -1;

        // <cropID>
        private Action<int> onTouchAddCallback = null;

        public void Initialize(int cropID, Action<int> onTouchAddCallback)
        {
            base.Initialize();
            this.cropID = cropID;
            this.onTouchAddCallback = onTouchAddCallback;

            RefreshUI();
        }

        private void RefreshUI()
        {
            UserData mainUser = GameInstance.MainUser;
            cropNameText.text = ResourceUtility.GetCropNameLocalKey(cropID);
            cropPriceText.text = new CalculateCropPrice(cropID, 1, mainUser.storageData.level).cropPrice.ToNumberString();
            UpdateUI();
        }

        private void UpdateUI()
        {
            UserData mainUser = GameInstance.MainUser;
            ownCountText.text = mainUser.seedPocketData.seedStorage[cropID].ToNumberString();
            new SetSprite(iconImage, ResourceUtility.GetSeedIconKey(cropID));
        }

        public void OnTouchAddButton()
        {
            onTouchAddCallback?.Invoke(cropID);
            UpdateUI();
        }
    }
}