using UnityEngine;

namespace ProjectF.UI
{
    public class MonoBehaviourUI : MonoBehaviour
    {
        private RectTransform rectTransform = null;
        public RectTransform RectTransform {
            get {
                if(rectTransform == null)
                    rectTransform = transform as RectTransform;
                return rectTransform;
            }
        }

        protected virtual void Awake() { }

        protected virtual void Initialize() { }
        protected virtual void Release() { }

        public void StretchUI()
        {
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.offsetMin = Vector2.zero;
            RectTransform.offsetMax = Vector2.zero;
            RectTransform.sizeDelta = Vector2.zero;
        }
    }
}
