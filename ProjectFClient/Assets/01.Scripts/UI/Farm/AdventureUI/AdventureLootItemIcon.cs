using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Adventure
{
    public class AdventureLootItemIcon : PoolableBehaviourUI
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _countText;

        public void Initialize(Sprite iconVisual, int count)
        {
            _itemIcon.sprite = iconVisual;
            _countText.text = count.ToString();
        }
    }
}
