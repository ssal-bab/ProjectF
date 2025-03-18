
namespace ProjectF.Networks.Packets
{
    public class PlantRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "Plant";
        public override string Post => POST;

        public int cropID = 0;
        public int fieldID = 0;

        public PlantRequest(int cropID, int fieldID)
        {
            this.cropID = cropID;
            this.fieldID = fieldID;
        }
    }

    public class PlantResponse : ResponsePacket
    {
        public int cropID = 0;
    }
}