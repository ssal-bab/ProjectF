using System.Collections.Generic;
using ProjectF.Networks;
using ProjectF.Networks.Packets;

namespace ProjectF.Networks.Packets
{
    public class FarmerSalesRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;
        public const string POST = "FarmerSales";
        public override string Post => POST;

        public IEnumerable<string> farmerUUID;

        public FarmerSalesRequest(IEnumerable<string> farmerUUID)
        {
            this.farmerUUID = farmerUUID;
        }
    }

    public class FarmerSalesResponse : ResponsePacket
    {
    }
}