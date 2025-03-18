
namespace ProjectF.Networks.Packets
{
    public class PlantCropRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "PlantCrop";
        public override string Post => POST;

        public int fieldGroupID = 0;
        public int fieldID = 0;
        public int cropID = 0;

        public PlantCropRequest(int fieldGroupID, int fieldID,  int cropID)
        {
            this.fieldGroupID = fieldGroupID;
            this.fieldID = fieldID;
            this.cropID = cropID;
        }
    }

    public class PlantCropResponse : ResponsePacket
    {
        public int cropID = 0;
    }
}