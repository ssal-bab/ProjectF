using System;
using System.Collections.Generic;
using H00N.DataTables;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProjectF.StringUtility;

namespace ProjectF.UI.Adventures
{
    public class AdventureAreaPopupUI : PoolableBehaviourUI
    {
        [SerializeField] TMP_Text areaNameText = null;
        [SerializeField] Image areaImage = null;
        
        [Header("Time Info")]
        [SerializeField] TMP_Text adventureTimeText = null;
        
        [Header("Loot Info")]
        [SerializeField] AddressableAsset<AdventureLootElementUI> lootElementUIPrefab = null;
        [SerializeField] Transform cropLootElementContainer = null;
        [SerializeField] Transform eggLootElementContainer = null;

        [Header("Farmer Info")]
        [SerializeField] List<AdventureFarmerElementUI> farmerElementUIList = null;

        [Header("Button")]
        [SerializeField] TMP_Text adventureButtonText = null;

        private int areaID = -1;
        private Action<int, List<string>, AdventureAreaPopupUI> startAdventureCallback = null;

        public async void Initialize(int areaID, Action<int, List<string>, AdventureAreaPopupUI> startAdventureCallback)
        {
            base.Initialize();
            this.areaID = areaID;
            this.startAdventureCallback = startAdventureCallback;

            await lootElementUIPrefab.InitializeAsync();
            RefreshUI();
        }

        protected override void Release()
        {
            base.Release();
            cropLootElementContainer.DespawnAllChildren();
            eggLootElementContainer.DespawnAllChildren();
        }

        private void RefreshUI()
        {
            areaNameText.text = ResourceUtility.GetAdventureAreaNameLocalKey(areaID);
            new SetSprite(areaImage, ResourceUtility.GetAdventureAreaImageKey(areaID));

            UserData mainUser = GameInstance.MainUser;
            if(mainUser.adventureData.adventureAreas.TryGetValue(areaID, out int level) == false)
                return;

            AdventureLevelTableRow tableRow = DataTableManager.GetTable<AdventureLevelTable>().GetRow(areaID, level);
            adventureTimeText.text = GetTimeString(ETimeStringType.Flexiblehms, 0, 0, 0, tableRow.adventureTime);               

            List<AdventureCropLootTableRow> cropLootTableRowList = DataTableManager.GetTable<AdventureCropLootTable>().GetRowList(areaID, level);
            cropLootElementContainer.DespawnAllChildren();
            foreach(var cropLootTableRow in cropLootTableRowList)
            {
                AdventureLootElementUI cropLootElement = PoolManager.Spawn(lootElementUIPrefab, cropLootElementContainer);
                cropLootElement.InitializeTransform();
                cropLootElement.Initialize(ELootItemType.Crop, cropLootTableRow.cropID);
            }

            List<AdventureEggLootTableRow> eggLootTableRowList = DataTableManager.GetTable<AdventureEggLootTable>().GetRowList(areaID, level);
            eggLootElementContainer.DespawnAllChildren();
            foreach(var eggLootTableRow in eggLootTableRowList)
            {
                AdventureLootElementUI eggLootElement = PoolManager.Spawn(lootElementUIPrefab, eggLootElementContainer);
                eggLootElement.InitializeTransform();
                eggLootElement.Initialize(ELootItemType.Egg, eggLootTableRow.eggID);
            }

            if(mainUser.adventureData.adventureFinishDatas.TryGetValue(areaID, out DateTime startTime))
                StartCoroutine(this.LoopRoutine(1f, UpdateUI));
            else
                adventureButtonText.text = "탐험 시작";
                
            for(int i = 0; i < farmerElementUIList.Count; i++)
                farmerElementUIList[i].Initialize(areaID, i);
        }

        private bool UpdateUI()
        {
            UserData mainUser = GameInstance.MainUser;
            if(mainUser.adventureData.adventureFinishDatas.TryGetValue(areaID, out DateTime finishTime) == false)
                return true;

            double remainTime = (finishTime - GameInstance.ServerTime).TotalSeconds;
            if(remainTime < 0)
            {
                adventureButtonText.text = "탐험 완료!";
                return true;
            }
            else
                adventureButtonText.text = $"탐험 중\n({GetTimeString(ETimeStringType.TotalHoursAndMinutesSeconds, 0, 0, 0, remainTime)})";

            return false;
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
        }

        public void OnTouchStartAdventureButton()
        {
            List<string> farmerList = new List<string>();
            foreach(var farmerElementUI in farmerElementUIList)
            {
                if(farmerElementUI.TryGetFarmerID(out string farmerID))
                    farmerList.Add(farmerID);
            }
            
            startAdventureCallback?.Invoke(areaID, farmerList, this);
        }
    }
}
