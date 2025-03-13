using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [Route(NetworkDefine.NEST_ROUTE)]
    public class NestController : PacketControllerBase
    {
        public NestController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory) { }

        [HttpPost(HatchEggRequest.POST)]
        public async Task<ActionResult<HatchEggResponse>> HatchEggRequestPost([FromBody]HatchEggRequest req)
        {
            HatchEggProcessor processor = new HatchEggProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(NestUpgradeRequest.POST)]
        public async Task<ActionResult<NestUpgradeResponse>> NestUpgradeRequestPost([FromBody]NestUpgradeRequest req)
        {
            NestUpgradeProcessor processor = new NestUpgradeProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}
