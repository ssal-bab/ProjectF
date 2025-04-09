using Cysharp.Threading.Tasks;
using H00N.FSM;
using H00N.Resources.Pools;
using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    public class FarmerGetSeedAction : FarmerFSMAction
    {
        [SerializeField] FSMState moveState = null;
        [SerializeField] PoolReference seedPrefab = null;

        public override async void EnterState()
        {
            base.EnterState();

            Farm farm = FarmManager.Instance.MainFarm;
            if(farm.CropQueue.CropQueueValid == false)
            {
                brain.SetAsDefaultState();
                return;
            }

            // Storage로 향해있던 타겟을 제거한다.
            aiData.PopTarget();

            // 이전 타겟이 Field가 아니면 return;
            Field field = aiData.CurrentTarget as Field;
            if(field == null)
            {
                brain.SetAsDefaultState();
                return;
            }

            bool result = await brain.LockBrainAsync(async () => {
                // 서버상에서는 씨앗을 수령하는 순간 Queue에서 Dequeue됨과 동시에 밭에 심어지는 것.
                int cropID = await ProcessPlantAsync(field.FieldGroupID, field.FieldID);
                if(cropID == -1)
                {
                    brain.SetAsDefaultState();
                    return;
                }

                Seed seed = PoolManager.Spawn<Seed>(seedPrefab);
                seed.Initialize(cropID);
                seed.transform.position = brain.transform.position;

                // 들고 바로 이동한다.
                Farmer.GrabItem(seed);
                brain.ChangeState(moveState);
            });

            if(result == false)
            {
                brain.SetAsDefaultState();
                return;
            }
        }

        private async UniTask<int> ProcessPlantAsync(int fieldGroupID, int fieldID)
        {
            PlantCropResponse response = await NetworkManager.Instance.SendWebRequestAsync<PlantCropResponse>(new PlantCropRequest(fieldGroupID, fieldID));

            if (response.result != ENetworkResult.Success)
            {
                // 크롭 큐는 민감하다. 실패했다면 타이틀로 돌려주자.
                Debug.LogError("[FarmerGetSeedAction::ProcessPlantCropRequest] There is a problem with the crop queue");
                return -1;
            }

            Farm farm = FarmManager.Instance.MainFarm;
            if (farm.CropQueue.CropQueueValid == false || farm.CropQueue.PeekSlot().cropID != response.cropID)
            {
                // 크롭 큐는 민감하다. 실패했다면 타이틀로 돌려주자.
                Debug.LogError("[FarmerGetSeedAction::ProcessPlantCropRequest] There is a problem with the crop queue");
                return -1;
            }

            farm.CropQueue.DequeueCropData();
            new ApplyPlantCrop(GameInstance.MainUser.fieldGroupData, fieldGroupID, fieldID, response.cropID);

            return response.cropID;
        }
    }
}
