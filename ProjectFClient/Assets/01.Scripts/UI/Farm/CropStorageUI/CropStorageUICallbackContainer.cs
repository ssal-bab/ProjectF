using System;

namespace ProjectF.UI.Farms
{
    public class CropStorageUICallbackContainer : UICallbackContainer
    {
        public Action<int> SellCropCallback = null;
        
        public CropStorageUICallbackContainer(Action<int> sellCropCallback)
        {
            SellCropCallback = sellCropCallback;
        }
    }
}