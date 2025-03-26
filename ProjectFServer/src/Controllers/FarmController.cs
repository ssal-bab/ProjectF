using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost(PlantCropRequest.POST)]
        public async Task<ActionResult<PlantCropResponse>> PlantCropRequestPost([FromBody]PlantCropRequest req)
        {
            PlantCropProcessor processor = new PlantCropProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(HarvestCropRequest.POST)]
        public async Task<ActionResult<HarvestCropResponse>> HarvestCropRequestPost([FromBody]HarvestCropRequest req)
        {
            HarvestCropProcessor processor = new HarvestCropProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(UpdateFarmRequest.POST)]
        public async Task<ActionResult<UpdateFarmResponse>> UpdateFarmRequestPost([FromBody]UpdateFarmRequest req)
        {
            UpdateFarmProcessor processor = new UpdateFarmProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(FieldGroupUpgradeRequest.POST)]
        public async Task<ActionResult<FieldGroupUpgradeResponse>> FieldGroupUpgradeRequestPost([FromBody]FieldGroupUpgradeRequest req)
        {
            FieldGroupUpgradeProcessor processor = new FieldGroupUpgradeProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}
