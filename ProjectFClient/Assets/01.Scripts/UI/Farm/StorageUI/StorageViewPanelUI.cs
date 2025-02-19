using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public abstract class StorageViewPanelUI : MonoBehaviourUI
    {
        [SerializeField] CanvasGroup canvasGroup = null;

        public virtual void Initialize(UserStorageData userStorageData, StorageUICallbackContainer callbackContainer) { }

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