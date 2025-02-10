using ProjectF.Datas;

namespace ProjectF.UI.Farms
{
    public abstract class CropStorageInfoUI : ToggleUI
    {
        public virtual void Initialize(UserCropStorageData userCropStorageData, CropStorageUICallbackContainer callbackContainer, CropStorageInfoPanel panel) { }
    }
}