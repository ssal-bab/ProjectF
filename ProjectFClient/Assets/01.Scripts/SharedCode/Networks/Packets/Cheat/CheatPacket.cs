namespace ProjectF.Networks.Packets
{
    public class CheatRequest : RequestPacket
    {
        public override string Route => NetworkDefine.CHEAT_ROUTE;

        public const string POST = "Cheat";
        public override string Post => POST;

        public string command;

        public CheatRequest(string command)
        {
            this.command = command;
        }
    }

    public class CheatResponse : ResponsePacket
    {
        public string response;
    }
}