using System;
using System.Collections.Generic;

namespace ProjectF.Networks.Packets
{
    public class AdventureStartRequest : RequestPacket
    {
        public override string Route => NetworkDefine.ADVENTURE_ROUTE;

        public const string POST = "AdventureStart";
        public override string Post => POST;

        public int areaID;
        public List<string> farmerList;

        public AdventureStartRequest(int areaID, List<string> farmerList) 
        { 
            this.areaID = areaID;
            this.farmerList = farmerList;
        }
    }

    public class AdventureStartResponse : ResponsePacket
    {
        public DateTime finishTime;
    }
}