using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI
{
    public class UpgradeInfoUI : MonoBehaviourUI
    {
        [SerializeField] RectTransform anchor = null;
        [SerializeField] TMP_Text descText = null;
        [SerializeField] TMP_Text currentValueText = null;
        [SerializeField] TMP_Text nextValueText = null;

        public void Initialize(string desc, string currentValue, string nextValue)
        {
            descText.text = desc;
            currentValueText.text = currentValue;
            nextValueText.text = nextValue;
            RebuildAnchorLayout();
        }

        private async void RebuildAnchorLayout()
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            LayoutRebuilder.ForceRebuildLayoutImmediate(anchor);
        }
    }
}
