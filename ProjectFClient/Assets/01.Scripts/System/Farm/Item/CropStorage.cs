using H00N.Resources.Pools;

namespace ProjectF.Farms
{
    public class CropStorage : ItemStorage
    {
        public override void StoreItem(Item item)
        {
            base.StoreItem(item);
            PoolManager.Despawn(item);
        }
    }
}