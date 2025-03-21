namespace ProjectF.Networks.Packets
{
    public class ServerConnectionRequest : RequestPacket
    {
        public override string Route => NetworkDefine.STANDARD_ROUTE;

        public const string POST = "ServerConnection";
        public override string Post => POST;

        public ServerConnectionRequest() { }
    }

    public class ServerConnectionResponse : ResponsePacket
    {
        public bool connection = false;
    }
}
