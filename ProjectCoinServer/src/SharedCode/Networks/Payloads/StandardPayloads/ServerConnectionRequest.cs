
namespace ProjectCoin.Networks.Payloads
{
    public class ServerConnectionRequest : RequestPayload
    {
        public override string Route => NetworkDefine.STANDARD_ROUTE;

        public const string POST = "server_connection";
        public override string Post => POST;

        public ServerConnectionRequest() { }
    }
}
