using System.Collections.Generic;
using H00N.DataTables;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks;
using ProjectFServer.Networks.Packets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Adventure
{
    public class AdventurePopupUI : PoolableBehaviourUI
    {
        [SerializeField] private Image _areaVisual;
        [SerializeField] private TextMeshProUGUI _areaNameText;
        [SerializeField] private AddressableAsset<ExploreFarmerInfoElementUI> _farmerElementUIPrefab;
        [SerializeField] private AddressableAsset<ExploreFarmerInfoPopupUI> _farmerPopupUIPrefab;
        [SerializeField] private RectTransform _farmerListContent;
        private const int LOOT_ITEM_COUNT = 7;
        [SerializeField] private Image[] _lootItemVisualArr = new Image[LOOT_ITEM_COUNT];

        private AdventureData _adventureData;
        private List<string> _exploreFarmerList;

        public void Initialize(AdventureData data)
        {
            _adventureData = data;
            _exploreFarmerList = new List<string>();

            var adventureTable = DataTableManager.GetTable<AdventureTable>();
            var adventureRow = adventureTable.GetRow(data.adventureAreaID);

            new SetSprite(_areaVisual, ResourceUtility.GetAdventureAreaImageKey(data.adventureAreaID));
            _areaNameText.text = adventureRow.nameLocalKey;

            RegisterLootItemIcon();
            SpawnExploreFarmerElement();
        }

        private void RegisterLootItemIcon()
        {
            var lootTable = DataTableManager.GetTable<AdventureLootTable>();
            var lootRow = lootTable.GetRow(_adventureData.adventureAreaID);

            #region RegisterVisual
            new SetSprite(_lootItemVisualArr[0], ResourceUtility.GetMaterialIconKey(lootRow.item1));
            new SetSprite(_lootItemVisualArr[1], ResourceUtility.GetMaterialIconKey(lootRow.item2));
            new SetSprite(_lootItemVisualArr[2], ResourceUtility.GetMaterialIconKey(lootRow.item3));
            new SetSprite(_lootItemVisualArr[3], ResourceUtility.GetMaterialIconKey(lootRow.item4));

            new SetSprite(_lootItemVisualArr[4], ResourceUtility.GetSeedIconKey(lootRow.seed1));
            new SetSprite(_lootItemVisualArr[5], ResourceUtility.GetSeedIconKey(lootRow.seed2));
            new SetSprite(_lootItemVisualArr[6], ResourceUtility.GetSeedIconKey(lootRow.seed3));
            #endregion
        }

        private async void SpawnExploreFarmerElement()
        {
            await _farmerElementUIPrefab.InitializeAsync();
            await _farmerPopupUIPrefab.InitializeAsync();

            // 탐험 중이면 선택 못하게 해야함. 그건 Element 내부에서 처리
            var farmerData = GameInstance.MainUser.farmerData;
            Debug.Log(farmerData.farmerList.Values.Count);
            foreach (var data in farmerData.farmerList.Values)
            {
                var element = PoolManager.Spawn<ExploreFarmerInfoElementUI>(_farmerElementUIPrefab, _farmerListContent);
                element.Initialize(data, RegisterExploreFarmer, UnRegisterExploreFarmer, _farmerPopupUIPrefab);
                element.StretchRect();
                element.RectTransform.sizeDelta = new Vector2(0, 120);
            }
        }

        public void RegisterExploreFarmer(string uuid)
        {
            if (_exploreFarmerList.Contains(uuid)) return;

            _exploreFarmerList.Add(uuid);
        }

        public void UnRegisterExploreFarmer(string uuid)
        {
            if (!_exploreFarmerList.Contains(uuid)) return;

            _exploreFarmerList.Remove(uuid);
        }

        public async void StartAdventure()
        {
            // 패킷작업
            var req = new AdventureStartRequest(_adventureData.adventureAreaID, _exploreFarmerList);
            var response = await NetworkManager.Instance.SendWebRequestAsync<AdventureStartResponse>(req);

            Debug.Log(response);

            if (response.result != ENetworkResult.Success)
            {
                Debug.LogError("Critical Error!");
                return;
            }

            var adventureData = GameInstance.MainUser.adventureData;

            adventureData.inAdventureAreaList.Add(_adventureData.adventureAreaID, response.adventureStartTime);
            adventureData.inExploreFarmerList.Add(_adventureData.adventureAreaID, _exploreFarmerList);

            foreach (var uuid in _exploreFarmerList)
            {
                adventureData.allFarmerinExploreList.Add(uuid);
            }

            OnTouchCloseButton();
        }

        public void OnTouchCloseButton()
        {
            base.Release();
            PoolManager.Despawn(this);
        }
    }
}
