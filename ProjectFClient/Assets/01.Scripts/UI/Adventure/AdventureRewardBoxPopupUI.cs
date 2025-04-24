using System.Collections.Generic;
using H00N.DataTables;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventureRewardBoxPopupUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<AdventureRewardBoxElementUI> rewardBoxElementUIPrefab = null;

        public new async void Initialize()
        {
            base.Initialize();
            await rewardBoxElementUIPrefab.InitializeAsync();
        }
    }
}
