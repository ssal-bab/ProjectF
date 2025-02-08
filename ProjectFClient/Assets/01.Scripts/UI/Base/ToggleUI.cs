using H00N.OptOptions;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectF.UI
{
    public class ToggleUI : MonoBehaviourUI
    {
        [SerializeField] OptOption<GameObject> toggleObjectOption = null;
        [SerializeField] bool toggleValue = false;

        [Space(10f)]
        public UnityEvent<bool> OnValueChangedEvent = null;
        public bool ToggleValue => toggleValue;

        public void Toggle()
        {
            SetToggle(!toggleValue);
        }

        public void SetToggle(bool toggle)
        {
            toggleValue = toggle;
            OnValueChangedEvent?.Invoke(toggleValue);

            toggleObjectOption[!toggleValue]?.SetActive(false);
            toggleObjectOption[toggleValue]?.SetActive(true);
        }

        #if UNITY_EDITOR
        private int tempToggleValue = -1;
        private void OnValidate()
        {
            int Bool2Int(bool value) => value ? 1 : 0;
            if(tempToggleValue == -1)
                tempToggleValue = Bool2Int(toggleValue);

            if(tempToggleValue == Bool2Int(toggleValue))
                return;

            tempToggleValue = Bool2Int(toggleValue);
            SetToggle(toggleValue);
        }
        #endif
    }
}
