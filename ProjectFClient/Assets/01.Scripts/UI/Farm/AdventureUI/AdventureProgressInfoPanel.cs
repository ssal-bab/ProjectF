using System;
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
using H00N.Extensions;

namespace ProjectF.UI.Adventure
{
    public abstract class ProgressInfoGroup
    {
        public abstract void Active();
        public abstract void DeActive();
    }
    [Serializable]
    public class LootItemScrollGroup : ProgressInfoGroup
    {
        public GameObject lootItemScroll;
        public GameObject recieveButton;
        public RectTransform content;
        public AddressableAsset<AdventureLootItemIcon> lootItemIconUIPrefab;

        public override void Active()
        {
            lootItemScroll.SetActive(true);
            recieveButton.SetActive(true);
        }

        public override void DeActive()
        {
            lootItemScroll.SetActive(false);
            recieveButton.SetActive(false);
        }
    }
    [Serializable]
    public class ProgressTimerGroup : ProgressInfoGroup
    {
        public GameObject progressTimer;
        public GameObject adventureCancleButton;
        public TextMeshProUGUI timerText;

        public override void Active()
        {
            progressTimer.SetActive(true);
            adventureCancleButton.SetActive(true);
        }

        public override void DeActive()
        {
            progressTimer.SetActive(false);
            adventureCancleButton.SetActive(false);
        }
    }

    public class AdventureProgressInfoPanel : PoolableBehaviourUI
    {
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Image _areaVisual;
        [SerializeField] private TextMeshProUGUI _areaNameText;

        [SerializeField] private LootItemScrollGroup _lootItemScrollGroup;
        [SerializeField] private ProgressTimerGroup _progressTimerGroup;

        private AdventureData _adventureData;
        private AdventureLootTable _adventureLootTable;

        private bool _isCompleteExplore = false;
        private double _currentRemainSeconds;
        private float _elapsedTime;

        public async void Initialize(AdventureData adventureData)
        {
            _adventureData = adventureData;

            Debug.Log($"{GameInstance.MainUser.adventureData.inAdventureAreaList[adventureData.adventureAreaID]}, {DateTime.Now}");

            var req = new CheckAdventureProgressRequest(adventureData.adventureAreaID);
            var response = await NetworkManager.Instance.SendWebRequestAsync<CheckAdventureProgressResponse>(req);

            if (response.result != ENetworkResult.Success)
            {
                Debug.LogError("CriticalError!");
            }

            _adventureLootTable = DataTableManager.GetTable<AdventureLootTable>();
            _isCompleteExplore = response.isCompleteExplore;

            if (response.isCompleteExplore)
            {
                InitializeLootItemScroll(_adventureLootTable.GetRow(adventureData.adventureAreaID));
            }
            else
            {
                InitializeProgressTimer(response.remainTime);
            }
        }

        private async void InitializeLootItemScroll(AdventureLootTableRow row)
        {
            _lootItemScrollGroup.Active();
            _progressTimerGroup.DeActive();

            _lootItemScrollGroup.lootItemIconUIPrefab.Initialize();

            var req = new AdventureResultRequest(_adventureData.adventureAreaID);
            var response = await NetworkManager.Instance.SendWebRequestAsync<AdventureResultResponse>(req);

            if (response.result != ENetworkResult.Success)
            {
                Debug.LogError("Critical Error!");
                return;
            }

            var storageData = GameInstance.MainUser.storageData;
            var seedPocketData = GameInstance.MainUser.seedPocketData;

            var materialStorage = storageData.materialStorage;
            var seedStorage = seedPocketData.seedStorage;

            foreach (var data in response.materialLootInfo)
            {
                var lootItemIcon = await PoolManager.SpawnAsync<AdventureLootItemIcon>(_lootItemScrollGroup.lootItemIconUIPrefab.Key, _lootItemScrollGroup.content);
                lootItemIcon.Initialize(ResourceUtility.GetMaterialIcon(data.materialItemID), data.itemCount);
                lootItemIcon.StretchRect();

                if (materialStorage.ContainsKey(data.materialItemID))
                {
                    materialStorage[data.materialItemID] += data.itemCount;
                }
                else
                {
                    materialStorage.Add(data.materialItemID, data.itemCount);
                }
            }

            foreach (var data in response.seedLootInfo)
            {
                var lootItemIcon = await PoolManager.SpawnAsync<AdventureLootItemIcon>(_lootItemScrollGroup.lootItemIconUIPrefab.Key, _lootItemScrollGroup.content);
                lootItemIcon.Initialize(ResourceUtility.GetMaterialIcon(data.seedItemID), data.itemCount);
                lootItemIcon.StretchRect();

                if (seedStorage.ContainsKey(data.seedItemID))
                {
                    seedStorage[data.seedItemID] += data.itemCount;
                }
                else
                {
                    seedStorage.Add(data.seedItemID, data.itemCount);
                }
            }
        }

        public void OnTouchRecieveButton()
        {
            if (_isCompleteExplore)
            {
                var adventureData = GameInstance.MainUser.adventureData;
                adventureData.inAdventureAreaList.Remove(_adventureData.adventureAreaID);
                var list = adventureData.inExploreFarmerList[_adventureData.adventureAreaID];

                foreach (var farmerUUID in list)
                {
                    adventureData.allFarmerinExploreList.Remove(farmerUUID);
                }

                adventureData.inExploreFarmerList.Remove(_adventureData.adventureAreaID);

                base.Release();

                _lootItemScrollGroup.content.DespawnAllChildren();
                _isCompleteExplore = false;
                PoolManager.DespawnAsync(this);
            }
        }

        private void InitializeProgressTimer(double remainSeconds)
        {
            _progressTimerGroup.Active();
            _lootItemScrollGroup.DeActive();

            Debug.Log(remainSeconds);
            _currentRemainSeconds = remainSeconds;
        }

        private void Update()
        {
            if (!_isCompleteExplore) return;

            if (_currentRemainSeconds <= 0) return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= 1f)
            {
                int hours = (int)(_currentRemainSeconds / 3600);
                int minute = (int)((_currentRemainSeconds % 3600) / 60);
                int seconds = (int)(_currentRemainSeconds % 60);

                _progressTimerGroup.timerText.text = $"{hours} : {minute} : {seconds}";

                _elapsedTime = 0;
                _currentRemainSeconds -= 1;
            }
        }
    }
}
