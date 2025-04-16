using H00N.Resources;
using H00N.Resources.Pools;

namespace ProjectF.UI.Cheats
{
    public class CheatButtonUI : MonoBehaviourUI
    {
        public async void OnTouchCheatButton()
        {
            await ResourceManager.LoadResourceAsync("CheatPopupUI");
            CheatPopupUI cheatPopupUI = PoolManager.Spawn<CheatPopupUI>("CheatPopupUI", GameDefine.TopPopupFrame);
            cheatPopupUI.StretchRect();
            cheatPopupUI.Initialize();
        }
    }
}
