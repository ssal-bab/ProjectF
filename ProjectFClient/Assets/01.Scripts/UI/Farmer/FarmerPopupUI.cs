using System;
using System.Collections.Generic;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using TMPro;
using UnityEngine;

namespace ProjectF.UI.Farmers
{
    public partial class FarmerPopupUI : PoolableBehaviourUI
    {
        [SerializeField] TMP_Text titleText = null;
        [SerializeField] GameObject selectModeButtonObject = null;

        [Space(10f)]
        [SerializeField] AddressableAsset<FarmerInfoPopupUI> farmerInfoPopupUIPrefab = null;
        [SerializeField] AddressableAsset<FarmerElementUI> farmerElementUIPrefab = null;
        [SerializeField] Transform container = null;

        private bool selectMode = false;
        private List<FarmerElementUI> selectedFarmerElementUIList = null;
        private Action<List<FarmerElementUI>> confirmCallback = null;

        private Color selectedColor = Color.white;

        public new async void Initialize()
        {
            base.Initialize();
            selectMode = false;
            selectedFarmerElementUIList ??= new List<FarmerElementUI>();

            await farmerInfoPopupUIPrefab.InitializeAsync();
            await farmerElementUIPrefab.InitializeAsync();

            RefreshUI();
        }

        private void RefreshUI()
        {
            container.DespawnAllChildren();
            UserFarmerData userFarmerData = GameInstance.MainUser.farmerData;
            foreach(var farmerData in userFarmerData.farmerDatas.Values)
            {
                FarmerElementUI ui = PoolManager.Spawn<FarmerElementUI>(farmerElementUIPrefab, container);
                ui.Initialize(farmerData.farmerUUID, selectedColor, OnTouchFarmerElementUI);
            }
        }

        private void OnTouchFarmerElementUI(FarmerElementUI ui)
        {
            if(selectMode)
            {
                ui.SetSelected(true);
                selectedFarmerElementUIList.Add(ui);
                return;
            }

            FarmerInfoPopupUI infoPopupUI = PoolManager.Spawn<FarmerInfoPopupUI>(farmerInfoPopupUIPrefab, container);
            infoPopupUI.StretchRect();
            infoPopupUI.Initialize(ui.FarmerUUID, SellFarmer);
        }

        private async void SellFarmer(string farmerUUID, FarmerInfoPopupUI infoPopupUI)
        {
            // Sell
            RefreshUI();
            PoolManager.Despawn(infoPopupUI);
        }

        public void OnTouchSelectModeButton()
        {
            SetSelectMode(!selectMode);
        }

        public void OnTouchConfirmButton()
        {
            confirmCallback?.Invoke(selectedFarmerElementUIList);
            SetSelectMode(false);
        }

        public void SetSelectMode(bool selectMode)
        {
            this.selectMode = selectMode;
            
            if(selectMode) // 선택모드 시작
            {
                selectedFarmerElementUIList.Clear();
            }
            else // 선택모드 해제
            {
                selectedFarmerElementUIList.ForEach(ui => ui.SetSelected(false));
                selectedFarmerElementUIList.Clear();
            }
        }

        public void OnTouchCloseButton()
        {
            base.Release();
            PoolManager.Despawn(this);
        }
    }
}
