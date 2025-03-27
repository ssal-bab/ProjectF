using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using H00N.Extensions;
using H00N.Resources;
using H00N.Resources.Pools;
using ProjectF.Datas;
using UnityEngine;

namespace ProjectF.Farms
{
    public class FarmerQuarters : FarmerTargetableBehaviour
    {
        // 소환 딜레이
        private const int SPAWN_INTERVAL_MILLISECONDS = 1000;
        private const float FARMER_REST_UPDATE_DELAY = 0.1f;

        [SerializeField] Transform entranceTransform = null;
        public override Vector3 TargetPosition => entranceTransform.position;

        [SerializeField] AddressableAsset<Farmer> farmerPrefab = null;
        private Dictionary<string, Farmer> farmerList = null;
        private List<Farmer> cagedFarmerList = null;

        public async UniTask InitializeAsync()
        {
            await farmerPrefab.InitializeAsync();
            
            cagedFarmerList = new List<Farmer>();
            farmerList = new Dictionary<string, Farmer>();

            Dictionary<string, FarmerData> userFarmerList = GameInstance.MainUser.farmerData.farmerList;
            string[] farmerUUIDList = userFarmerList.Keys.ToArray();
            AddFarmers(farmerUUIDList);

            StartCoroutine(this.LoopRoutine(FARMER_REST_UPDATE_DELAY, RestFarmer));
        }

        public void AddFarmers(params string[] farmerUUIDList)
        {
            Dictionary<string, FarmerData> userFarmerList = GameInstance.MainUser.farmerData.farmerList;
            foreach(string farmerUUID in farmerUUIDList)
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

            UncageAllFarmerAsync(farmerUUIDList);
        }

        private async void UncageAllFarmerAsync(params string[] farmerUUIDList)
        {
            foreach(string farmerUUID in farmerUUIDList)
            {
                if(farmerList.TryGetValue(farmerUUID, out Farmer farmer) == false)
                    continue;

                farmer.gameObject.SetActive(true);
                await UniTask.Delay(SPAWN_INTERVAL_MILLISECONDS);
            }
        }

        public void CageFarmer(string farmerUUID)
        {
            if(farmerList.TryGetValue(farmerUUID, out Farmer farmer) == false)
            {
                Debug.LogError($"[FarmerQuarters::CageFarmer] Invalid farmerUUID. farmerUUID : {farmerUUID}");
                return;
            }

            farmer.gameObject.SetActive(false);
            cagedFarmerList.Add(farmer);
        }

        public void UncageFarmer(string farmerUUID)
        {
            if (farmerList.TryGetValue(farmerUUID, out Farmer farmer) == false)
            {
                Debug.LogError($"[FarmerQuarters::UncageFarmer] Invalid farmerUUID. farmerUUID : {farmerUUID}");
                return;
            }

            farmer.AIData.isResting = false;
            farmer.gameObject.SetActive(true);
        }

        private void RestFarmer()
        {
            for(int i = cagedFarmerList.Count - 1; i >= 0; --i)
            {
                Farmer farmer = cagedFarmerList[i];
                if(farmer == null)
                    continue;

                farmer.Stat.IncreaseHP(FARMER_REST_UPDATE_DELAY);
                if(farmer.Stat.CurrentHP < farmer.Stat[EFarmerStatType.Health])
                    continue;

                cagedFarmerList.RemoveAt(i);
                UncageFarmer(farmer.FarmerUUID);
            }
        }
    }
}
        