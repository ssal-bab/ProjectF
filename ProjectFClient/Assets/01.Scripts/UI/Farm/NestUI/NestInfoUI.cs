using ProjectF.Datas;

namespace ProjectF.UI.Farms
{
    public abstract class NestInfoUI : ToggleUI
    {
        public virtual void Initialize(UserNestData userNestData, NestUICallbackContainer callbackContainer, NestInfoPanelUI panel) { }
    }
}