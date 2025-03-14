using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [ApiController]
    [Route(NetworkDefine.CHEAT_ROUTE)]
    public class CheatController : PacketControllerBase
    {
        public CheatController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory) { }

        [HttpPost(CheatRequest.POST)]
        public async Task<ActionResult<CheatResponse>> CheatRequestPost([FromBody]CheatRequest req)
        {
            CheatProcessor processor = new CheatProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}
