
namespace ProjectF.Networks.Packets
{
    public class HarvestRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "Harvest";
        public override string Post => POST;

        public int fieldGroupID = 0;
        public int fieldID = 0;

        public HarvestRequest(int fieldGroupID, int fieldID)
        {
            this.fieldGroupID = fieldGroupID;
            this.fieldID = fieldID;
        }
    }

    public class HarvestResponse : ResponsePacket
    {
        public int productCropID = 0;
    }
}