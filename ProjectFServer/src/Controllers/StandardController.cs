using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [Route(NetworkDefine.STANDARD_ROUTE)]
    public class StandardController : PacketControllerBase
    {
        public StandardController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory) { }

        [HttpPost(ServerConnectionRequest.POST)]
        public async Task<ActionResult<ServerConnectionResponse>> ServerConnectionRequestPost([FromBody]ServerConnectionRequest req)
        {
            ServerConnectionProcessor processor = new ServerConnectionProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(LoginRequest.POST)]
        public async Task<ActionResult<LoginResponse>> LoginRequestPost([FromBody]LoginRequest req)
        {
            LoginProcessor processor = new LoginProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}
