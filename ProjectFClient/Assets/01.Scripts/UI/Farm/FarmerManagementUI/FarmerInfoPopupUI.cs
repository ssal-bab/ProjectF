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
    }

    public class FarmerInfoPopupUI : PoolableBehaviourUI
    {
        private const int STAT_COUNT = 4;
        private FarmerData currentFarmerData;
        private Farmer currentFarmer;
        [SerializeField] private TextMeshProUGUI farmerLevelText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private FarmerStatInfo[] farmerStatInfoArr = new FarmerStatInfo[STAT_COUNT];

        public void Initialize(Farmer farmer, FarmerData farmerData)
        {
            base.Initialize();

            currentFarmerData = farmerData;
            currentFarmer = farmer;

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

            // 체력이 10이라 해서 수확하는 작물 수가 10이 아님. 하지만 그걸 계산하는 수식이 현재는 없어 깡 수치로 박아넣었다. 추후에 수정 필요
            var currentLevelStatDictionary = new GetFarmerStat(tableRow, currentLevel).statDictionary;
            var nextLevelStatDictionary = new GetFarmerStat(tableRow, nextLevel).statDictionary;

            foreach(var info in farmerStatInfoArr)
            {
                var type = info.statType;
                string preview = $"{currentLevelStatDictionary[type]} {StringUtility.ColorTag(GameDefine.AbleBehavioutColor, $">> {nextLevelStatDictionary[type]}")}";

                info.statText.text = $"{GetStatInfoText(type)} {preview}";
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

        private string GetStatInfoText(EFarmerStatType statType)
        {
            switch (statType)
            {
                case EFarmerStatType.None:
                    return string.Empty;
                case EFarmerStatType.MoveSpeed:
                    return "목표까지 이동하는 속도";
                case EFarmerStatType.Health:
                    return "쉬지않고 수확하는 작물 수";
                case EFarmerStatType.FarmingSkill:
                    return "한번에 수확하는 작물 수";
                case EFarmerStatType.AdventureSkill:
                    return "탐험중 작물, 재료 추가 획득 확률 증가\n";
                default:
                    return string.Empty;
            }
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.DespawnAsync(this);
        }

        protected override void Release()
        {
            base.Release();
        }
    }
}
