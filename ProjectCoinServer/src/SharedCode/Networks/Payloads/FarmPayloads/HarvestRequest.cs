namespace ProjectCoin.Networks.Payloads
{
    public class HarvestRequest : RequestPayload
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "harvest";
        public override string Post => POST;

        public int fieldID = 0;

        public HarvestRequest(int fieldID)
        {
            this.fieldID = fieldID;
        }
    }
}