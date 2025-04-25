using System;
using UnityEngine;
using ProjectF.Datas;

namespace ProjectF.UI.Adventures
{
    public class AdventureLootElementUI : PoolableBehaviourUI
    {
        public void Initialize(ELootItemType lootItemType, int id, int count)
        {
            base.Initialize();

            if(count == -1)
            {
                // 텍스트를 띄우지 않는다.
            }
        }
    }
}
