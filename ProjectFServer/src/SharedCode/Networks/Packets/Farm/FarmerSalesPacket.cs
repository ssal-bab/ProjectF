using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectF.Networks;
using ProjectF.Networks.Packets;

namespace ProjectFServer.Networks.Packets
{
    public class FarmerSalesRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;
        public const string POST = "FarmerSales";
        public override string Post => POST;
        public string farmerUUID = string.Empty;
        public int salesAllowance = 0;

        public FarmerSalesRequest(string farmerUUID, int salesAllowance)
        {
            this.farmerUUID = farmerUUID;
            this.salesAllowance = salesAllowance;
        }
    }

    public class FarmerSalesResponse : ResponsePacket
    {
    }
}