using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class SeedPocketPopupUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<CropQueueElementUI> cropQueueElementUIPrefab;
        [SerializeField] ScrollRect cropQueueScrollView = null;

        [SerializeField] AddressableAsset<SeedElementUI> seedElementUIPrefab;
        [SerializeField] ScrollRect seedScrollView = null;

        private CropQueue deltaQueue = null;
        private List<CropQueueActionData> cropQueueActionDataList = null;
        private Dictionary<int, int> tempSeedUsedInfo = null;

        public new async void Initialize()
        {
            base.Initialize();
            await cropQueueElementUIPrefab.InitializeAsync();
            await seedElementUIPrefab.InitializeAsync();

            deltaQueue ??= new CropQueue();
            deltaQueue.Clear();
            
            cropQueueActionDataList ??= new List<CropQueueActionData>();
            cropQueueActionDataList.Clear();

            tempSeedUsedInfo ??= new Dictionary<int, int>();
            tempSeedUsedInfo.Clear();

            RefreshUI();
        }

        private void RefreshUI()
        {
            List<CropQueueSlot> cropQueue = GameInstance.MainUser.seedPocketData.cropQueue;

            cropQueueScrollView.content.DespawnAllChildren();
            foreach(CropQueueSlot cropQueueSlot in cropQueue)
            {
                deltaQueue.EnqueueCrop(cropQueueSlot.cropID, cropQueueSlot.count);

                CropQueueElementUI ui = PoolManager.Spawn<CropQueueElementUI>(cropQueueElementUIPrefab, cropQueueScrollView.content);
                ui.InitializeTransform();
                ui.Initialize(deltaQueue.LastSlot(), RemoveFromCropQueue);
            }

            UserData mainUser = GameInstance.MainUser;
            seedScrollView.content.DespawnAllChildren();
            foreach(int cropID in mainUser.seedPocketData.seedStorage.Keys)
            {
                SeedElementUI ui = PoolManager.Spawn<SeedElementUI>(seedElementUIPrefab, seedScrollView.content);
                ui.InitializeTransform();
                ui.Initialize(cropID, GetSeedCount, AddToCropQueue);
            }
        }

        private void RemoveFromCropQueue(CropQueueElementUI ui, CropQueueSlot cropQueueSlot)
        {
            int index = deltaQueue.IndexOf(cropQueueSlot);
            if(index == -1)
                return;

            RecordAction(ECropQueueActionType.Remove, cropQueueSlot.cropID, index, 1);
            deltaQueue.RemoveFromCropQueue(index);

            if(cropQueueSlot.count <= 0)
                PoolManager.Despawn(ui);
        }

        private void AddToCropQueue(int cropID)
        {
            if(GetSeedCount(cropID) <= 0)
                return;

            CropQueueSlot lastSlot = deltaQueue.LastSlot();
            RecordAction(ECropQueueActionType.Enqueue, cropID, cropID, 1);
            deltaQueue.EnqueueCrop(cropID);

            if(lastSlot != null && lastSlot.cropID == cropID)
                return;

            CropQueueElementUI ui = PoolManager.Spawn<CropQueueElementUI>(cropQueueElementUIPrefab, cropQueueScrollView.content);
            ui.InitializeTransform();
            ui.Initialize(deltaQueue.LastSlot(), RemoveFromCropQueue);
        }

        private int GetSeedCount(int cropID)
        {
            tempSeedUsedInfo.TryGetValue(cropID, out int seedUsedCount);
            GameInstance.MainUser.seedPocketData.seedStorage.TryGetValue(cropID, out int seedCount);
            return seedCount - seedUsedCount;
        }

        private void RecordAction(ECropQueueActionType actionType, int cropID, int target, int count)
        {
            cropQueueActionDataList.Add(new CropQueueActionData(actionType, target, count));
            
            tempSeedUsedInfo.TryGetValue(cropID, out int seedUsedCount);
            if (actionType == ECropQueueActionType.Enqueue)
                tempSeedUsedInfo[cropID] = seedUsedCount + count;
            else if(actionType == ECropQueueActionType.Remove)
                tempSeedUsedInfo[cropID] = seedUsedCount - count;
        }

        public async void OnTouchCloseButton()
        {
            await ApplyCropQueueAction();

            Release();
            PoolManager.Despawn(this);
        }

        private async UniTask ApplyCropQueueAction()
        {
            if(cropQueueActionDataList.Count <= 0)
                return;

            ApplyCropQueueActionResponse response = await NetworkManager.Instance.SendWebRequestAsync<ApplyCropQueueActionResponse>(new ApplyCropQueueActionRequest(cropQueueActionDataList));
            if(response.result != ENetworkResult.Success)
                return;

            new ApplyCropQueueAction(GameInstance.MainUser, response.verifiedActionDataList);
            FarmManager.Instance.MainFarm.SetCropQueue(GameInstance.MainUser.seedPocketData.cropQueue);
        }
    }
}
