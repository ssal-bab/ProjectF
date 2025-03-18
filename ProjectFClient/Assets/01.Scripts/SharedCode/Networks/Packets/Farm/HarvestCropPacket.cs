
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class HarvestCropRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "HarvestCrop";
        public override string Post => POST;

        public int fieldGroupID;
        public int fieldID;

        public HarvestCropRequest(int fieldGroupID, int fieldID)
        {
            this.fieldGroupID = fieldGroupID;
            this.fieldID = fieldID;
        }
    }

    public class HarvestCropResponse : ResponsePacket
    {
        public int productCropID;
        public ECropGrade cropGrade;
    }
}