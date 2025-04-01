using System;
using H00N.Resources.Pools;
using ProjectF.Farms;
using TMPro;
using UnityEngine;
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectFServer.Networks.Packets;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.UI.Farms
{
    [Serializable]
    public class FarmerStatInfo
    {
        public EFarmerStatType statType;
        public TextMeshProUGUI statText;
        public string unit;
    }

    public class FarmerInfoPopupUI : PoolableBehaviourUI
    {
        private const int STAT_COUNT = 4;
        private FarmerData currentFarmerData;
        private Farmer currentFarmer;
        [SerializeField] private TextMeshProUGUI farmerLevelText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private FarmerStatInfo[] farmerStatInfoArr = new FarmerStatInfo[STAT_COUNT];

        public void Initialize(FarmerData farmerData)
        {
            base.Initialize();

            currentFarmerData = farmerData;
            
            var farm = FarmManager.Instance.MainFarm;
            currentFarmer = farm.FarmerQuarters.GetFarmerByUUID(farmerData.farmerUUID);

            RefreshUI(farmerData);
        }

        public void RefreshUI(FarmerData farmerData)
        {
            // MaxLevel인지지 판단 필요
            int currentLevel = farmerData.level;
            int nextLevel = farmerData.level + 1;
            
            farmerLevelText.text = $"{currentLevel} {StringUtility.ColorTag(GameDefine.AbleBehavioutColor, $">> {nextLevel}")}";

            var statTable = DataTableManager.GetTable<FarmerStatTable>();
            var tableRow = statTable.GetRow(farmerData.farmerID);

            var currentLevelStatDictionary = new GetFarmerStat(tableRow, currentLevel).statDictionary;
            var nextLevelStatDictionary = new GetFarmerStat(tableRow, nextLevel).statDictionary;

            foreach(var info in farmerStatInfoArr)
            {
                var type = info.statType;

                int currentValue = new CalculateFarmerProductivity(type, currentLevelStatDictionary[type]).value;
                int nextValue = new CalculateFarmerProductivity(type, nextLevelStatDictionary[type]).value;

                string preview = $"{currentValue}{info.unit} {StringUtility.ColorTag(GameDefine.AbleBehavioutColor, $">> {nextValue}{info.unit}")}";

                info.statText.text = $"{ResourceUtility.GetStatDescriptionLocakKey(type)} {preview}";
            }

            var farmerLevelupGoldTable = DataTableManager.GetTable<FarmerLevelupGoldTable>();
            int price = new CalculateFarmerLevelupGold(farmerLevelupGoldTable, farmerData.rarity, currentLevel).value;

            int haveMoney = GameInstance.MainUser.monetaData.gold;

            string color = haveMoney >= price ? GameDefine.AbleBehavioutColor : GameDefine.UnAbleBehaviourColor;
            
            priceText.text = $"<color=#{color}>{price}</color> / {haveMoney}";
        }

        public async void ChangeFarmerLevel()
        {
            if(!GameInstance.MainUser.farmerData.farmerList.ContainsKey(currentFarmerData.farmerUUID))
            {
                Debug.LogError("일꾼을 찾을 수 없습니다.");
                return;
            }

            var levelupGoldTable = DataTableManager.GetTable<FarmerLevelupGoldTable>();
            var farmerRarity = currentFarmerData.rarity;
            var level = currentFarmerData.level;

            int price = new CalculateFarmerLevelupGold(levelupGoldTable, farmerRarity, level).value;

            if(GameInstance.MainUser.monetaData.gold < price)
            {
                Debug.LogError("골드가 충분하지 않습니다.");
                return;
            }

            var req = new FarmerLevelupRequest(currentFarmerData.farmerUUID);
            var response = await NetworkManager.Instance.SendWebRequestAsync<FarmerLevelupResponse>(req);

            if(response.result != ENetworkResult.Success) 
            {
                Debug.LogError("Critical Error!");
                return;
            }

            GameInstance.MainUser.monetaData.gold -= price;
            currentFarmerData.level += 1;

            FarmerStatTable statTable = DataTableManager.GetTable<FarmerStatTable>();
            FarmerStatTableRow statRow = statTable.GetRow(currentFarmerData.farmerID);

            currentFarmer.Stat.SetData(statRow, currentFarmerData.level + 1);
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
        }

        protected override void Release()
        {
            base.Release();
        }
    }
}
