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
using ProjectF.UI;

namespace ProjectF
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
        private const string UPGRADE_COLOR = "64A980";
        private const string ERROR_COLOR = "FF6E6E";
        private FarmerData _currentFarmerData;
        private Farmer _currentFarmer;
        [SerializeField] private TextMeshProUGUI farmerLevelText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private FarmerStatInfo[] farmerStatInfoArr = new FarmerStatInfo[STAT_COUNT];

        public void Initialize(Farmer farmer, FarmerData farmerData)
        {
            base.Initialize();

            _currentFarmerData = farmerData;
            _currentFarmer = farmer;

            RefreshUI(farmerData);
        }

        public void RefreshUI(FarmerData farmerData)
        {
            // MaxLevel인지지 판단 필요
            int currentLevel = farmerData.level;
            int nextLevel = farmerData.level + 1;

            farmerLevelText.text = $"{currentLevel} <color=#{UPGRADE_COLOR}>>> {nextLevel}";

            var statTable = DataTableManager.GetTable<FarmerStatTable>();

            // 체력이 10이라 해서 수확하는 작물 수가 10이 아님. 하지만 그걸 계산하는 수식이 현재는 없어 깡 수치로 박아넣었다. 추후에 수정 필요
            var currentLevelStatDictionary = statTable.GetFarmerStat(farmerData.farmerID, currentLevel);
            var nextLevelStatDictionary = statTable.GetFarmerStat(farmerData.farmerID, nextLevel);

            foreach(var info in farmerStatInfoArr)
            {
                var type = info.statType;
                string preview = $"{currentLevelStatDictionary[type]} <color=#{UPGRADE_COLOR}>>> {nextLevelStatDictionary[type]}</color>";

                info.statText.text = $"{GetStatInfoText(type)} {preview}";
            }

            var farmerConfigTable = DataTableManager.GetTable<FarmerLevelupGoldTable>();

            float price = farmerConfigTable.BaseGoldDictionary[farmerData.rarity] * farmerConfigTable.MultiplierGoldDictionary[farmerData.rarity] * currentLevel;
            price = Mathf.Floor(price);

            int haveMoney = GameInstance.MainUser.monetaData.gold;

            string color = haveMoney >= price ? UPGRADE_COLOR : ERROR_COLOR;
            
            priceText.text = $"<color=#{color}>{price}</color> / {haveMoney}";
        }

        public async void ChangeFarmerLevel()
        {
            var req = new FarmerLevelupRequest(_currentFarmerData.farmerUUID, _currentFarmerData.level + 1);
            var response = await NetworkManager.Instance.SendWebRequestAsync<FarmerLevelupResponse>(req);

            if(response.result == ENetworkResult.DataNotFound) 
            {
                Debug.Log("일꾼을 찾을 수 없습니다.");
                return;
            }

            if(response.result == ENetworkResult.DataNotEnough)
            {
                Debug.Log("골드가 충분하지 않습니다.");
                return;
            }

            FarmerStatTable statTable = DataTableManager.GetTable<FarmerStatTable>();
            FarmerStatTableRow statRow = statTable.GetRow(_currentFarmerData.farmerID);

            _currentFarmer.Stat.SetData(statRow, _currentFarmerData.level + 1);
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
