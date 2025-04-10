using System;
using UnityEngine;

namespace ProjectF.UI.Adventures
{
    public class AdventureAreaElementUI : MonoBehaviourUI
    {
        [SerializeField] int areaID = -1;
        private Action<int> onTouchCallback = null;

        public void Initialize(Action<int> onTouchCallback)
        {
            base.Initialize();
            this.onTouchCallback = onTouchCallback;
        }

        public void OnTouchThis()
        {
            if(areaID == -1)
                return;

            onTouchCallback?.Invoke(areaID);
        }
    }
}
