using ProjectCoin.Farms.Helpers;

namespace ProjectCoin.Farms
{
    public class Egg : Item
    {
        private EggStorage currentStorage = null;
        public override bool TargetEnable => base.TargetEnable && currentStorage == null;

        protected override FarmerTargetableBehaviour GetDeliveryTarget()
        {
            // Delivery 타겟을 요청할 때 스택에 남은 타겟이 있으면 밭으로 간주
            // 밭을 그대로 넘겨준다.
            if(Holder != null && Holder.AIData.CurrentTarget != null)
                return Holder.AIData.CurrentTarget;

            // Holder가 null이거나 Holder의 타겟 스택에 남은 타겟이 없으면 EggStorage로 배달을 요청한다.
            Farm farm = new GetBelongsFarm(transform).currentFarm;
            return farm.EggStorage;
        }

        public void SetStorage(EggStorage storage)
        {
            currentStorage = storage;
        }
    }
}