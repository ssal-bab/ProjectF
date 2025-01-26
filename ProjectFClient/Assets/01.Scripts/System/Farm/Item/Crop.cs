using ProjectF.Farms.Helpers;

namespace ProjectF.Farms
{
    public class Crop : Item
    {
        protected override FarmerTargetableBehaviour GetDeliveryTarget()
        {
            Farm farm = new GetBelongsFarm(transform).currentFarm;
            return farm.CropStorage;
        }
    }
}