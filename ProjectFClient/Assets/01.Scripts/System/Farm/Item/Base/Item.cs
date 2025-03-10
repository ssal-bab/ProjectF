using Cysharp.Threading.Tasks;
using H00N.Resources;
using ProjectF.Datas;

namespace ProjectF.Farms
{
    public abstract class Item : FarmerTargetableBehaviour
    {
        public FarmerTargetableBehaviour DeliveryTarget => GetDeliveryTarget();

        private Farmer holder = null;
        public Farmer Holder => holder;

        private int id = 0;
        public int ID => id;

        public virtual void Initialize(int id)
        {
            this.id = id;
        }

        public void SetHolder(Farmer farmer)
        {
            holder = farmer;
        }

        protected abstract FarmerTargetableBehaviour GetDeliveryTarget();
    }
}