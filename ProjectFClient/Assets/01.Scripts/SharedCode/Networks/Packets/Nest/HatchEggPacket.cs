using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class HatchEggRequest : RequestPacket
    {
        public override string Route => NetworkDefine.NEST_ROUTE;

        public const string POST = "HatchEgg";
        public override string Post => POST;

        public int eggIndex;

        public HatchEggRequest(int eggIndex) 
        { 
            this.eggIndex = eggIndex;
        }
    }

    public class HatchEggResponse : ResponsePacket
    {
        public int hatchedEggIndex;
        public FarmerData farmerData;
    }
}