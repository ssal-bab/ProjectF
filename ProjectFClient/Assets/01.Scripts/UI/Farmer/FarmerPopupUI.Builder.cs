using System;
using System.Collections.Generic;
using System.Linq;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF.UI.Farmers
{
    public partial class FarmerPopupUI
    {
        public class Builder
        {
            private AddressableAsset<FarmerPopupUI> prefab = null;
            
            private string title = "일꾼";
            private Color selectedColor = Color.white;
            private bool selectModeButtonActive = true;
            private Action<List<string>> confirmCallback = null;

            public Builder(AddressableAsset<FarmerPopupUI> prefab)
            {
                this.prefab = prefab;
                title = "일꾼";
                selectedColor = Color.white;
                selectModeButtonActive = true;
                confirmCallback = null;
            }

            public Builder SetTitle(string title)
            {
                this.title = title;
                return this;
            }

            public Builder SetSelectedColor(Color selectedColor)
            {
                this.selectedColor = selectedColor;
                return this;
            }

            public Builder SetSelectModeButton(bool selectModeButtonActive)
            {
                this.selectModeButtonActive = selectModeButtonActive;
                return this;
            }

            public Builder SetConfirmCallback(Action<List<string>> confirmCallback)
            {
                this.confirmCallback = confirmCallback;
                return this;
            }

            public FarmerPopupUI Build()
            {
                FarmerPopupUI ui = PoolManager.Spawn<FarmerPopupUI>(prefab);
                ui.selectedColor = selectedColor;
                ui.Initialize();
                ui.titleText.SetText(title);
                ui.selectModeButtonObject.SetActive(selectModeButtonActive);
                ui.confirmCallback = confirmCallback;
                return ui;
            }
        }
    }
}
