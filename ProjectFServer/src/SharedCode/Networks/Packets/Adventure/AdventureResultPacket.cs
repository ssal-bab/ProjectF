using System.Collections.Generic;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class AdventureResultRequest : RequestPacket
    {
        public override string Route => NetworkDefine.ADVENTURE_ROUTE;

        public const string POST = "AdventureResult";
        public override string Post => POST;

        public int areaID = 0;
        public AdventureResultRequest(int areaID)
        {
            this.areaID = areaID;
        }
    }

    public class AdventureResultResponse : ResponsePacket
    {
        public AdventureResultPack resultPack;
    }
}