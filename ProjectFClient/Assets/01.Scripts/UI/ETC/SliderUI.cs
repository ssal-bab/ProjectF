using TMPro;
using UnityEngine;

namespace ProjectF.UI
{
    public class SliderUI : MonoBehaviourUI
    {
        [SerializeField] TMP_Text countText = null;
        [SerializeField] RectTransform sliderFillRect = null;

        public void RefreshUI(int max, int current)
        {
            countText.text = $"{current}/{max}";

            Vector2 anchorMax = sliderFillRect.anchorMax;
            anchorMax.x = Mathf.Max(current / (float)max, 0);
            sliderFillRect.anchorMax = anchorMax;
        }
    }
}
