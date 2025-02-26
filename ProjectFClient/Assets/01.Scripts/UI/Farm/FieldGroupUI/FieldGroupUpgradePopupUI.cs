using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class FieldGroupUpgradePopupUI : MonoBehaviourUI
    {
        [SerializeField] TMP_Text currentLevelText = null;
        [SerializeField] Image currentIconImage = null;
        
        [Space(10f)]
        [SerializeField] TMP_Text nextLevelText = null;
        [SerializeField] Image nextIconImage = null;

        [Space(10f)]
        [SerializeField] RarityUpgradeUI[] rarityUpgradeUIList = new RarityUpgradeUI[4];
        // [SerializeField]  // 재료 UI


        
    }
}
