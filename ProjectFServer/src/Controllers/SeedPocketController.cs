using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [Route(NetworkDefine.SEED_POCKET_ROUTE)]
    public class SeedPocketController : PacketControllerBase
    {
        public SeedPocketController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory) { }

        [HttpPost(ApplyCropQueueActionRequest.POST)]
        public async Task<ActionResult<ApplyCropQueueActionResponse>> ApplyCropQueueActionRequestPost([FromBody]ApplyCropQueueActionRequest req)
        {
            ApplyCropQueueActionProcessor processor = new ApplyCropQueueActionProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}
