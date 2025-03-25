using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Farms;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectF.UI.Farms
{
    public class SeedsPocketPopupUI : PoolableBehaviourUI
    {
        [SerializeField] AddressableAsset<CropQueueElementUI> cropQueueElementUIPrefab;
        [SerializeField] ScrollRect queueScrollView = null;

        [SerializeField] AddressableAsset<SeedElementUI> seedElementUIPrefab;
        [SerializeField] ScrollRect seedsScrollView = null;

        private CropQueue cropQueue = null;

        public void Initialize(Farm farm)
        {
            base.Initialize();
            cropQueueElementUIPrefab.Initialize();
            seedElementUIPrefab.Initialize();

            cropQueue = farm.CropQueue;
            RefreshUI();
        }

        private void RefreshUI()
        {
            queueScrollView.content.DespawnAllChildren();
            foreach(CropQueueSlot cropQueueSlot in cropQueue)
            {
                CropQueueElementUI ui = PoolManager.Spawn<CropQueueElementUI>(cropQueueElementUIPrefab, queueScrollView.content);
                ui.InitializeTransform();
                ui.Initialize(cropQueueSlot, RemoveFromCropQueue);
            }

            UserData mainUser = GameInstance.MainUser;
            seedsScrollView.content.DespawnAllChildren();
            foreach(int cropID in mainUser.seedsPocketData.seedsStorage.Keys)
            {
                SeedElementUI ui = PoolManager.Spawn<SeedElementUI>(seedElementUIPrefab);
                ui.InitializeTransform();
                ui.Initialize(cropID, AddToCropQueue);
            }
        }

        private void RemoveFromCropQueue(CropQueueSlot cropQueueSlot)
        {

        }

        private void AddToCropQueue(int cropID)
        {

        }
    }
}
