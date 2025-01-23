using H00N.Resources;
using H00N.DataTables;
using H00N.FSM;
using ProjectCoin.Datas;
using ProjectCoin.DataTables;
using ProjectCoin.Farms.Helpers;
using UnityEngine;

namespace ProjectCoin.Farms.AI
{
    public class FarmerGetEggAction : FarmerFSMAction
    {
        [SerializeField] FSMState liftState = null;

        public override void EnterState()
        {
            base.EnterState();

            Farm currentFarm = new GetBelongsFarm(brain.transform).currentFarm;
            if(currentFarm == null)
            {
                brain.SetAsDefaultState();
                return;
            }

            CropSO targetSeedData = aiData.currentSeedData;
            if(targetSeedData.TableRow.cropType != ECropType.Egg)
            {
                brain.SetAsDefaultState();
                return;
            }

            ItemSO eggItemData = ResourceManager.LoadResource<ItemSO>($"ItemData_{targetSeedData.TableRow.seedItemID}");
            Egg egg = currentFarm.EggStorage.GetEgg(eggItemData);
            if(egg == null || currentFarm.EggStorage.ConsumeItem(eggItemData) == false)
            {
                Debug.Log("필요한 알이 없음!");
                brain.SetAsDefaultState();
                return;
            }

            // Storage로 향해있던 타겟을 제거하고 Egg로 변경
            aiData.PopTarget();
            aiData.PushTarget(egg);
            brain.ChangeState(liftState);
        }
    }
}
