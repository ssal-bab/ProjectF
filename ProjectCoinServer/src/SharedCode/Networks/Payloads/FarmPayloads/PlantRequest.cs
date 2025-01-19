namespace ProjectCoin.Networks.Payloads
{
    public class PlantRequest : RequestPayload
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "plant";
        public override string Post => POST;

        public int cropID = 0;
        public int fieldID = 0;

        public PlantRequest(int cropID, int fieldID)
        {
            this.cropID = cropID;
            this.fieldID = fieldID;
        }
    }
}