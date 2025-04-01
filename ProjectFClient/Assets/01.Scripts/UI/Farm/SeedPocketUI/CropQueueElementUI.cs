using System;
using ProjectF.Farms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class CropQueueElementUI : PoolableBehaviourUI
    {
        [SerializeField] Image iconImage = null;
        [SerializeField] TMP_Text countText = null;

        private CropQueueSlot cropQueueSlot = null;
        private Action<CropQueueElementUI, CropQueueSlot> onTouchCallback = null;

        public void Initialize(CropQueueSlot cropQueueSlot, Action<CropQueueElementUI, CropQueueSlot> onTouchCallback)
        {
            base.Initialize();
            this.cropQueueSlot = cropQueueSlot;
            this.onTouchCallback = onTouchCallback;
            this.cropQueueSlot.OnCountChangedEvent += HandleCropQueueSlotCountChanged;

            RefreshUI();
        }

        private void RefreshUI()
        {
            new SetSprite(iconImage, ResourceUtility.GetSeedIconKey(cropQueueSlot.cropID));
            UpdateUI();
        }

        private void UpdateUI()
        {
            countText.text = cropQueueSlot.count.ToString();
        }

        private void HandleCropQueueSlotCountChanged(CropQueueSlot slot)
        {
            UpdateUI();
        }

        public void OnTouchThis()
        {
            onTouchCallback?.Invoke(this, cropQueueSlot);
        }
    }
}