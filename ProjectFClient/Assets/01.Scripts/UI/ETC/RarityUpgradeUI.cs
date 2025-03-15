using ProjectF.Datas;
using TMPro;
using UnityEngine;

namespace ProjectF.UI
{
    public class RarityUpgradeUI : MonoBehaviourUI
    {
        [SerializeField] TMP_Text rarityText = null;
        [SerializeField] TMP_Text currentValueText = null;
        [SerializeField] TMP_Text nextValueText = null;

        public void Initialize(ERarity rarity, float currentValue, float nextValue)
        {
            rarityText.text = ResourceUtility.GetRarityNameLocalKey(rarity); // localizing 적용 해야함
            currentValueText.text = $"{currentValue:0.##}%";
            nextValueText.text = $"{nextValue:0.##}%";
        }

        public void Initialize(ECropGrade rarity, float currentValue, float nextValue)
        {
            rarityText.text = ResourceUtility.GetCropGradeNameLocalKey(rarity); // localizing 적용 해야함
            currentValueText.text = $"{currentValue:0.##}%";
            nextValueText.text = $"{nextValue:0.##}%";
        }
    }
}
