using System;

namespace ProjectF.UI.Farms
{
    public class NestUICallbackContainer : UICallbackContainer
    {
        // <TargetCropStorageID, result>
        public Func<int, bool> UpgradeGoldCheckCallback = null;
        
        // <TargetCropStorageID, result>
        public Func<int, bool> SkipGemCheckCallback = null;
        
        // <TargetCropStorageID, result>
        public Func<int, bool> UpgradeMaterialCheckCallback = null;
        
        // <TargetCropStorageID, result>
        public Action<int> UpgradeCallback = null;

        public NestUICallbackContainer(Func<int, bool> upgradeGoldCheckCallback, Func<int, bool> forceUpgradeCheckCallback, Func<int, bool> upgradeMaterialCheckCallback, Action<int> upgradeCallback)
        {
            UpgradeGoldCheckCallback = upgradeGoldCheckCallback;
            SkipGemCheckCallback = forceUpgradeCheckCallback;
            UpgradeMaterialCheckCallback = upgradeMaterialCheckCallback;
            UpgradeCallback = upgradeCallback;
        }
    }
}