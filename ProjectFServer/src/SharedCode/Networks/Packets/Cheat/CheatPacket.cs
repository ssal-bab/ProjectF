namespace ProjectF.Networks.Packets
{
    public class CheatRequest : RequestPacket
    {
        public override string Route => NetworkDefine.CHEAT_ROUTE;

        public const string POST = "Cheat";
        public override string Post => POST;

        public string command;
        public string[] option;

        public CheatRequest(string command, params string[] option)
        {
            this.command = command;
            this.option = option;
        }
    }

    public class CheatResponse : ResponsePacket
    {
        public string response;
    }
}