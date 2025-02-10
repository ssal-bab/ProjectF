using System;

namespace ProjectF.UI.Farms
{
    public class CropStorageUICallbackContainer : UICallbackContainer
    {
        // <CropItemID>
        public Action<int> SellCropCallback = null;
        
        // <TargetCropStorageID, result>
        public Func<int, bool> UpgradeCostCheckCallback = null;
        
        // <TargetCropStorageID, result>
        public Func<int, bool> UpgradeMaterialCheckCallback = null;
        
        // <TargetCropStorageID, result>
        public Action<int> UpgradeCallback = null;

        public CropStorageUICallbackContainer(Action<int> sellCropCallback, Func<int, bool> upgradeCostCheckCallback, Func<int, bool> upgradeMaterialCheckCallback, Action<int> upgradeCallback)
        {
            SellCropCallback = sellCropCallback;
            UpgradeCostCheckCallback = upgradeCostCheckCallback;
            UpgradeMaterialCheckCallback = upgradeMaterialCheckCallback;
            UpgradeCallback = upgradeCallback;
        }
    }
}