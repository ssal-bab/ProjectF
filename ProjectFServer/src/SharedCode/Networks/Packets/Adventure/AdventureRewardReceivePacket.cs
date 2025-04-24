namespace ProjectF.Networks.Packets
{
    public class AdventureRewardReceiveRequest : RequestPacket
    {
        public override string Route => NetworkDefine.ADVENTURE_ROUTE;

        public const string POST = "AdventureRewardReceive";
        public override string Post => POST;

        public string adventureRewardUUID;
        public int index;

        public AdventureRewardReceiveRequest(string adventureRewardUUID, int index) 
        { 
            this.adventureRewardUUID = adventureRewardUUID;
            this.index = index;
        }
    }

    public class AdventureRewardReceiveResponse : ResponsePacket
    {

    }
}