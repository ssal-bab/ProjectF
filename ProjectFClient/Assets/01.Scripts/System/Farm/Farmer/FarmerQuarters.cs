using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.Farms
{
    public class FarmerQuarters : MonoBehaviour
    {
        // 소환 딜레이
        private const int SPAWN_INTERVAL_MILLISECONDS = 1000;

        [SerializeField] Transform entranceTransform = null;
        [SerializeField] AddressableAsset<Farmer> farmerPrefab = null;
        private Dictionary<string, Farmer> farmerList = null;

        public async UniTask InitializeAsync()
        {
            await farmerPrefab.InitializeAsync();

            farmerList = new Dictionary<string, Farmer>();
            Dictionary<string, FarmerData> userFarmerList = GameInstance.MainUser.farmerData.farmerList;

            foreach(string farmerUUID in userFarmerList.Keys)
            {
                FarmerData farmerData = userFarmerList[farmerUUID];
                if(farmerList.ContainsKey(farmerUUID))
                {
                    Debug.LogError($"[FarmerQuarters::Initialize] Multiple UUID already exists in farmer list. UUID: {farmerUUID}");
                    continue;
                }

                Farmer farmer = PoolManager.Spawn<Farmer>(farmerPrefab);
                farmer.transform.position = entranceTransform.position;
                farmer.Initialize(farmerData);
                farmer.gameObject.SetActive(false);

                farmerList.Add(farmerUUID, farmer);
            }

            UncageAllFarmerAsync();
        }

        private async void UncageAllFarmerAsync()
        {
            var keys = farmerList.Keys;
            foreach(string farmerUUID in keys)
            {
                if(farmerList.TryGetValue(farmerUUID, out Farmer farmer) == false)
                    continue;

                farmer.gameObject.SetActive(true);
                await UniTask.Delay(SPAWN_INTERVAL_MILLISECONDS);
            }
        }
    }
}
        