using ProjectF.Datas;
using ProjectF.Networks;
using ProjectF.Networks.Packets;

namespace ProjectFServer.Networks.Packets
{
    public class ReceiveAdventureResultRequest : RequestPacket
    {
        public override string Route => NetworkDefine.ADVENTURE_ROUTE;

        public const string POST = "ReceiveAdventureResult";
        public override string Post => POST;

        public int areaID = 0;
        public ReceiveAdventureResultRequest(int areaID)
        {
            this.areaID = areaID;
        }
    }

    public class ReceiveAdventureResultResponse : ResponsePacket
    {
        public AdventureResultPack resultPack;
    }
}