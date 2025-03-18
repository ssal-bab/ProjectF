using ProjectF.Datas;

namespace ProjectF.Farms
{
    public class Crop : Item
    {
        private int cropID = 0;
        public int CropID => cropID;

        private ECropGrade cropGrade = ECropGrade.None;
        public ECropGrade CropGrade => cropGrade;

        public void Initialize(int cropID, ECropGrade cropGrade)
        {
            this.cropID = cropID;
            this.cropGrade = cropGrade;
        }

        protected override FarmerTargetableBehaviour GetDeliveryTarget()
        {
            Farm farm = new GetBelongsFarm(transform).currentFarm;
            return farm.Storage;
        }
    }
}