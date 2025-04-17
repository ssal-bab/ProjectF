using System;
using System.Collections.Generic;
using ProjectF.Networks;
using ProjectF.Networks.Packets;

namespace ProjectF.Networks.Packets
{
      public class CheckAdventureProgressRequest : RequestPacket
      {
            public override string Route => NetworkDefine.ADVENTURE_ROUTE;

            public const string POST = "CheckAdventureProgress";
            public override string Post => POST;

            public int areaID = 0;
            public CheckAdventureProgressRequest(int areaID)
            {
                  this.areaID = areaID;
            }
      }

      public class CheckAdventureProgressResponse : ResponsePacket
      {
            public bool isCompleteExplore;
            public double remainTime;
      }
}