using System.Collections.Generic;

namespace ProjectF.Networks.Packets

    public class FarmerSellRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARMER_ROUTE;

        public const string POST = "FarmerSell";
        public override string Post => POST;

        public List<string> farmerList;

        public FarmerSellRequest(List<string> farmerList) 
        { 
            this.farmerList = farmerList;
        }
    }

    public class FarmerSellResponse : ResponsePacket
    {
        public Dictionary<int, int> earnedMoneta;
    }
}