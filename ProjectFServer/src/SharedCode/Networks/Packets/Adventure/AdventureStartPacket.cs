using System;
using System.Collections.Generic;
using ProjectF.Networks;
using ProjectF.Networks.Packets;

namespace ProjectF.Networks.Packets
{
      public class AdventureStartRequest : RequestPacket
      {
            public override string Route => NetworkDefine.ADVENTURE_ROUTE;

            public const string POST = "AdventureStart";
            public override string Post => POST;

            public int areaID = 0;
            public List<string> exploreFarmerList;

            public AdventureStartRequest(int areaID, List<string> exploreFarmerList)
            {
                  this.areaID = areaID;
                  this.exploreFarmerList = exploreFarmerList;
            }
      }

      public class AdventureStartResponse : ResponsePacket
      {
            public DateTime adventureStartTime;
      }
}