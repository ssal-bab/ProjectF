using ProjectF.Datas;

namespace ProjectF.Farms
{
    public class Crop : Item
    {
        private int cropID = 0;
        public int CropID => cropID;

        private ECropGrade cropGrade = ECropGrade.None;
        public ECropGrade CropGrade => cropGrade;

        private int count = 0;
        public int Count => count;

        public void Initialize(int cropID, ECropGrade cropGrade, int count)
        {
            this.cropID = cropID;
            this.cropGrade = cropGrade;
            this.count = count;
        }

        protected override FarmerTargetableBehaviour GetDeliveryTarget()
        {
            Farm farm = new GetBelongsFarm(transform).currentFarm;
            return farm.Storage;
        }
    }
}