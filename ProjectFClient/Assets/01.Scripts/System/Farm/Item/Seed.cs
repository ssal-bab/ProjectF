using ProjectCoin.Farms.Helpers;

namespace ProjectCoin.Farms
{
    public class Seed : Item
    {
        protected override FarmerTargetableBehaviour GetDeliveryTarget()
        {
            // Delivery 타겟을 요청할 때 스택에 남은 타겟이 있으면 밭으로 간주
            // 밭을 그대로 넘겨준다.
            if(Holder != null && Holder.AIData.CurrentTarget != null)
                return Holder.AIData.CurrentTarget;

            // 그렇지 않으면 뭔가 잘못된 상태
            return null;
        }
    }
}