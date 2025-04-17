namespace ProjectF.Networks.Packets
{
    public class AdventureAreaUpgradeRequest : RequestPacket
    {
        public override string Route => NetworkDefine.FARM_ROUTE;

        public const string POST = "AdventureAreaUpgrade";
        public override string Post => POST;

        public int areaID;

        public AdventureAreaUpgradeRequest(int areaID) 
        { 
            this.areaID = areaID;
        }
    }

    public class AdventureAreaUpgradeResponse : ResponsePacket
    {
        public int currentLevel;
    }
}