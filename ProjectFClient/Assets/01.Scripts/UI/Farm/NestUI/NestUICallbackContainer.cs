using System;
using Cysharp.Threading.Tasks;

namespace ProjectF.UI.Farms
{
    public class NestUICallbackContainer : UICallbackContainer
    {
        // <TargetEggIndex, BornFarmerID>
        public Func<int, UniTask<int>> HatchCallback = null;
        
        // <TargetNestID, result>
        public Func<int, bool> UpgradeGoldCheckCallback = null;
        
        // <TargetNestID, result>
        public Func<int, bool> SkipGemCheckCallback = null;
        
        // <TargetNestID, result>
        public Func<int, bool> UpgradeMaterialCheckCallback = null;
        
        // <TargetNestID, result>
        public Action<int> UpgradeCallback = null;

        public NestUICallbackContainer(Func<int, UniTask<int>> hatchCallback, Func<int, bool> upgradeGoldCheckCallback, Func<int, bool> forceUpgradeCheckCallback, Func<int, bool> upgradeMaterialCheckCallback, Action<int> upgradeCallback)
        {
            HatchCallback = hatchCallback;
            UpgradeGoldCheckCallback = upgradeGoldCheckCallback;
            SkipGemCheckCallback = forceUpgradeCheckCallback;
            UpgradeMaterialCheckCallback = upgradeMaterialCheckCallback;
            UpgradeCallback = upgradeCallback;
        }
    }
}