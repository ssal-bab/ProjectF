using ProjectF.Datas;
using TMPro;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class FarmerStatUI : MonoBehaviourUI
    {
        [SerializeField] TMP_Text statNameText = null;
        [SerializeField] TMP_Text statValueText = null;

        public void Initialize(EFarmerStatType statType, float statValue)
        {
            statNameText.text = statType.ToString();
            statValueText.text = statValue.ToString();
        }
    }
}