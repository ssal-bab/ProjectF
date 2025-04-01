using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Farms;
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

        private CropQueue cropQueue = null;

        public async void Initialize(Farm farm)
        {
            base.Initialize();
            await cropQueueElementUIPrefab.InitializeAsync();
            await seedElementUIPrefab.InitializeAsync();

            cropQueue = farm.CropQueue;
            RefreshUI();
        }

        private void RefreshUI()
        {
            cropQueueScrollView.content.DespawnAllChildren();
            foreach(CropQueueSlot cropQueueSlot in cropQueue)
            {
                CropQueueElementUI ui = PoolManager.Spawn<CropQueueElementUI>(cropQueueElementUIPrefab, cropQueueScrollView.content);
                ui.InitializeTransform();
                ui.Initialize(cropQueueSlot, RemoveFromCropQueue);
            }

            UserData mainUser = GameInstance.MainUser;
            seedScrollView.content.DespawnAllChildren();
            foreach(int cropID in mainUser.seedPocketData.seedStorage.Keys)
            {
                SeedElementUI ui = PoolManager.Spawn<SeedElementUI>(seedElementUIPrefab, seedScrollView.content);
                ui.InitializeTransform();
                ui.Initialize(cropID, AddToCropQueue);
            }
        }

        private void RemoveFromCropQueue(CropQueueElementUI ui, CropQueueSlot cropQueueSlot)
        {
            cropQueue.RemoveFromCropQueue(cropQueueSlot);
            if(cropQueueSlot.count <= 0)
                PoolManager.Despawn(ui);
        }

        private void AddToCropQueue(int cropID)
        {
            CropQueueSlot lastSlot = cropQueue.LastSlot();
            cropQueue.EnqueueCrop(cropID);

            if(lastSlot != null && lastSlot.cropID == cropID)
                return;

            CropQueueElementUI ui = PoolManager.Spawn<CropQueueElementUI>(cropQueueElementUIPrefab, cropQueueScrollView.content);
            ui.InitializeTransform();
            ui.Initialize(cropQueue.LastSlot(), RemoveFromCropQueue);
        }

        public void OnTouchCloseButton()
        {
            Release();
            PoolManager.Despawn(this);
        }
    }
}
