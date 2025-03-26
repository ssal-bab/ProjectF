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

        public FarmerLevelupRequest(string farmerUUID)
        {
            this.farmerUUID = farmerUUID;
        }
    }

    public class FarmerLevelupResponse : ResponsePacket
    {
        public string farmerUUID = string.Empty;
    }
}