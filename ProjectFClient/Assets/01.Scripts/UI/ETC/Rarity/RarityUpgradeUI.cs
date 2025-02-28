using ProjectF.Datas;
using ProjectF.UI;
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
            rarityText.text = rarity.ToString(); // localizing 적용 해야함
            currentValueText.text = currentValue.ToString();
            nextValueText.text = nextValue.ToString();
        }

        public void Initialize(ECropGrade rarity, float currentValue, float nextValue)
        {
            rarityText.text = rarity.ToString(); // localizing 적용 해야함
            currentValueText.text = currentValue.ToString();
            nextValueText.text = nextValue.ToString();
        }
    }
}
