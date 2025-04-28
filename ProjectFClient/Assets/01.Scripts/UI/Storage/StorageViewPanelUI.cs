using UnityEngine;

namespace ProjectF.UI.Storages
{
    public abstract class StorageViewPanelUI : MonoBehaviourUI
    {
        [SerializeField] CanvasGroup canvasGroup = null;

        public virtual new void Initialize() { }
        public virtual new void Release() { }

        public void Show()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }
}