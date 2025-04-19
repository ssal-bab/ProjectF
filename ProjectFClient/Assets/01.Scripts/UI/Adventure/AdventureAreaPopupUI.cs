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

        [Space(10f)]
        [SerializeField] List<AdventureFarmerElementUI> farmerElementUIList = null;

        private int areaID = -1;

        public async void Initialize(int areaID)
        {
            base.Initialize();
            this.areaID = areaID;

            await lootElementUIPrefab.InitializeAsync();
            RefreshUI();
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

            for(int i = 0; i < farmerElementUIList.Count; i++)
            {
                farmerElementUIList[i].Initialize(areaID, i);
            }
        }
    }
}
