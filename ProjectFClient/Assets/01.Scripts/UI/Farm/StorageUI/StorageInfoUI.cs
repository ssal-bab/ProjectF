using ProjectF.Datas;

namespace ProjectF.UI.Farms
{
    public abstract class StorageInfoUI : ToggleUI
    {
        public virtual void Initialize(UserStorageData userStorageData, StorageUICallbackContainer callbackContainer, StorageInfoPanelUI panel) { }
    }
}