namespace ProjectF.Datas
{
    public class CropQueueActionData
    {
        public ECropQueueActionType actionType;
        public int target;
        public int count;

        public CropQueueActionData(ECropQueueActionType actionType, int target, int count)
        {
            this.actionType = actionType;
            this.target = target;
            this.count = count;
        }
    }
}