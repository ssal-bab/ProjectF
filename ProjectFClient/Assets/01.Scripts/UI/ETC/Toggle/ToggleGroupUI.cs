using UnityEngine;

namespace ProjectF.UI
{
    public class ToggleGroupUI : MonoBehaviourUI
    {
        [SerializeField] ToggleUI[] toggleUIList = null;
        [SerializeField] ToggleUI currentFocusedUI = null;

        public void SetToggle(int index)
        {
            if (toggleUIList == null)
                return;

            for (int i = 0; i < toggleUIList.Length; ++i)
            {
                ToggleUI ui = toggleUIList[i];
                if(i == index)
                {
                    currentFocusedUI = ui;
                    if(ui != null)
                        ui.SetToggle(true);
                }
                else
                {
                    if(ui != null)
                        ui.SetToggle(false);
                }
            }
        }

        public void SetToggle(ToggleUI toggleUI)
        {
            if(toggleUIList == null)
                return;

            currentFocusedUI = toggleUI;
            foreach(ToggleUI ui in toggleUIList)
            {
                if(ui != null)
                    ui.SetToggle(ui == currentFocusedUI);
            }
        }

        #if UNITY_EDITOR
        private ToggleUI tempFocusedUI = null;
        private bool initialized = false;
        private void OnValidate()
        {
            if(initialized == false)
            {
                tempFocusedUI = currentFocusedUI;
                SetToggle(tempFocusedUI);

                return;
            }


            if(tempFocusedUI == currentFocusedUI)
                return;

            tempFocusedUI = currentFocusedUI;
            SetToggle(tempFocusedUI);
        }
        #endif
    }
}
