using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
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

        private new async void Initialize()
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
            infoPopupUI.Initialize(ui.FarmerUUID, OnTouchSellFarmer, OnTouchUpgradeFarmer);
        }

        private async void OnTouchSellFarmer(string farmerUUID, FarmerInfoPopupUI infoPopupUI)
        {
            UserAdventureData adventureData = GameInstance.MainUser.adventureData;
            if(adventureData.adventureFarmerDatas.ContainsKey(farmerUUID))
            {
                // 탐험중입니다. 토스트 메세지.
                return;
            }

            await SellFarmers(new List<string> { farmerUUID });
            RefreshUI();
            PoolManager.Despawn(infoPopupUI);
        }

        private async void OnTouchUpgradeFarmer(string farmerUUID, FarmerInfoPopupUI infoPopupUI)
        {
            // Upgrade
            RefreshUI();
            PoolManager.Despawn(infoPopupUI);
        }

        public void OnTouchSelectModeButton()
        {
            SetSelectMode(!selectMode);
        }

        public async void OnTouchConfirmButton()
        {
            if (confirmCallback == null)
            {
                List<string> farmerList = new List<string>();
                UserAdventureData adventureData = GameInstance.MainUser.adventureData;
                foreach (var farmerElementUI in selectedFarmerElementUIList)
                {
                    if (adventureData.adventureFarmerDatas.ContainsKey(farmerElementUI.FarmerUUID))
                    {
                        // 탐험중입니다. 토스트 메세지.
                        return;
                    }

                    farmerList.Add(farmerElementUI.FarmerUUID);
                }

                await SellFarmers(farmerList);
                RefreshUI();
            }
            else
            {
                confirmCallback?.Invoke(selectedFarmerElementUIList);
            }

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

        private async UniTask SellFarmers(List<string> farmerList)
        {
            FarmerSellResponse response = await NetworkManager.Instance.SendWebRequestAsync<FarmerSellResponse>(new FarmerSellRequest(farmerList));
            if(response.result == ENetworkResult.Success)
                return;

            UserFarmerData farmerData = GameInstance.MainUser.farmerData;
            foreach(var monetaData in response.earnedMoneta)
            {
                farmerData.farmerMonetaStroage.TryGetValue(monetaData.Key, out int moneta);
                farmerData.farmerMonetaStroage[monetaData.Key] = moneta + monetaData.Value;
            }

            foreach(var farmerUUID in farmerList)
            {
                if(farmerData.farmerDatas.ContainsKey(farmerUUID) == false)
                    continue;

                farmerData.farmerDatas.Remove(farmerUUID);
            }
        }
    }
}
