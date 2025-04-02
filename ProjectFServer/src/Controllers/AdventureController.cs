using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using ProjectFServer.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [ApiController]
    [Route(NetworkDefine.ADVENTURE_ROUTE)]
    public class AdventureController : PacketControllerBase
    {
        public AdventureController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory)
        {
        }

        [HttpPost(AdventureStartRequest.POST)]
        public async Task<ActionResult<AdventureStartResponse>> AdventureStartRequestPost([FromBody]AdventureStartRequest req)
        {
            var processor = new AdventureStartProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(CheckAdventureProgressRequest.POST)]
        public async Task<ActionResult<CheckAdventureProgressResponse>> CheckAdventureProgressRequestPost([FromBody]CheckAdventureProgressRequest req)
        {
            var processor = new CheckAdventureProgressProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(AdventureResultRequest.POST)]
        public async Task<ActionResult<AdventureResultResponse>> AdventureResultRequestPost([FromBody]AdventureResultRequest req)
        {
            var processor = new AdventureResultProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(ReceiveAdventureResultRequest.POST)]
        public async Task<ActionResult<ReceiveAdventureResultResponse>> ReceiveAdventureResultRequestPost([FromBody]ReceiveAdventureResultRequest req)
        {
            var processor = new ReceiveAdventureResultProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}