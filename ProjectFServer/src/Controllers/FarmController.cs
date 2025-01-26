using Microsoft.AspNetCore.Mvc;
using ProjectF.Datas;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [ApiController]
    [Route(NetworkDefine.FARM_ROUTE)]
    public class FarmController : PacketControllerBase
    {
        public FarmController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory) { }

        [HttpPost(PlantRequest.POST)]
        public async Task<ActionResult<PlantResponse>> PlantCropRequestPost([FromBody]PlantRequest req)
        {
            PlantProcessor processor = new PlantProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(HarvestRequest.POST)]
        public async Task<ActionResult<HarvestResponse>> HarvestRequestPost([FromBody]HarvestRequest req)
        {
            HarvestProcessor processor = new HarvestProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}
