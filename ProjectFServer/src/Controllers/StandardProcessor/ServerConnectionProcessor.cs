using System.Threading.Tasks;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class ServerConnectionProcessor : PacketProcessorBase<ServerConnectionRequest, ServerConnectionResponse>
    {
        public ServerConnectionProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, ServerConnectionRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<ServerConnectionResponse> ProcessInternal()
        {
            return new ServerConnectionResponse() {
                result = ENetworkResult.Success,
                connection = true,
            };
        }
    }
}