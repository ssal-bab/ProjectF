using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectF.Networks;
using ProjectF.Networks.Packets;

namespace ProjectFServer.Networks.Packets
{
    public class FarmerLevelupRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "FarmerLevelup";
        public override string Post => POST;

        public string farmerUUID = string.Empty;
        public int targetLevel = 0;

        public FarmerLevelupRequest(string farmerUUID, int targetLevel)
        {
            this.farmerUUID = farmerUUID;
            this.targetLevel = targetLevel;
        }
    }

    public class FarmerLevelupResponse : ResponsePacket
    {
        public string farmerUUID = string.Empty;
        public int targetLevel = 0;
    }
}