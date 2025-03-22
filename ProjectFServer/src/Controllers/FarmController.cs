using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using ProjectFServer.Networks.Packets;
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

        [HttpPost(FarmerLevelupRequest.POST)]
        public async Task<ActionResult<FarmerLevelupResponse>> FarmerLevelupRequestPost([FromBody]FarmerLevelupRequest req)
        {
            FarmerLevelupProcessor processor = new FarmerLevelupProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(FarmerSalesRequest.POST)]
        public async Task<ActionResult<FarmerSalesResponse>> FarmerSalesRequesPost([FromBody]FarmerSalesRequest req)
        {
            FarmerSalesProcessor processor = new FarmerSalesProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}
