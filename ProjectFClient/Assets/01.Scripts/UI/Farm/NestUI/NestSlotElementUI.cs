using System;
using System.Collections;
using H00N.DataTables;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;

namespace ProjectF.UI.Farms
{
    public class NestSlotElementUI : MonoBehaviourUI
    {
        [SerializeField] NestEggInfoUI eggInfoUI = null;
        private Action openDetailInfoCallback = null;

        public void Initialize(EggHatchingData eggHatchingData, Action openDetailInfoCallback)
        {
            base.Initialize();
            this.openDetailInfoCallback = openDetailInfoCallback;
            
            eggInfoUI.Initialize(eggHatchingData);
        }

        public void OnTouchOpenDetailInfoButton()
        {
            openDetailInfoCallback?.Invoke();
        }
    }
}