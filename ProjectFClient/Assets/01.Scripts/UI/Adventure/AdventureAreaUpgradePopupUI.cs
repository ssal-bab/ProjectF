using System;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventureAreaUpgradePopupUI : PoolableBehaviourUI
    {
        private int areaID = -1;

        public void Initialize(int areaID)
        {
            base.Initialize();
            this.areaID = areaID;
        }
    }
}
