using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
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
using Random = UnityEngine.Random;

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

        private bool _canMoveTime = false;
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

            foreach (var data in response.materialLootInfo)
            {
                var lootItemIcon = await PoolManager.SpawnAsync<AdventureLootItemIcon>(_lootItemScrollGroup.lootItemIconUIPrefab.Key, _lootItemScrollGroup.content);
                lootItemIcon.Initialize(ResourceUtility.GetMaterialIcon(data.materialItemID), data.itemCount);
            }

            foreach (var data in response.seedLootInfo)
            {
                var lootItemIcon = await PoolManager.SpawnAsync<AdventureLootItemIcon>(_lootItemScrollGroup.lootItemIconUIPrefab.Key, _lootItemScrollGroup.content);
                lootItemIcon.Initialize(ResourceUtility.GetMaterialIcon(data.seedItemID), data.itemCount);
            }
        }

        private void InitializeProgressTimer(double remainSeconds)
        {
            _progressTimerGroup.Active();
            _lootItemScrollGroup.DeActive();

            _currentRemainSeconds = remainSeconds;
            Debug.Log(_currentRemainSeconds);
            _canMoveTime = true;
        }

        private void Update()
        {
            if (!_canMoveTime) return;

            if(_currentRemainSeconds <= 0) return;

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

        public void OnTouchCloseButton()
        {
            base.Release();
            _canMoveTime = false;
            PoolManager.DespawnAsync(this);
        }
    }
}
