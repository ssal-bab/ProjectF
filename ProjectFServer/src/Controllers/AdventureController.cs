using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [ApiController]
    [Route(NetworkDefine.ADVENTURE_ROUTE)]
    public class AdventureController : PacketControllerBase
    {
        public AdventureController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory) { }

        [HttpPost(AdventureAreaUpgradeRequest.POST)]
        public async Task<ActionResult<AdventureAreaUpgradeResponse>> AdventureAreaUpgradeRequestPost([FromBody]AdventureAreaUpgradeRequest req)
        {
            AdventureAreaUpgradeProcessor processor = new AdventureAreaUpgradeProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(AdventureStartRequest.POST)]
        public async Task<ActionResult<AdventureStartResponse>> AdventureStartRequestPost([FromBody]AdventureStartRequest req)
        {
            AdventureStartProcessor processor = new AdventureStartProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}