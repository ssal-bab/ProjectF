using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [Route(NetworkDefine.STORAGE_ROUTE)]
    public class StorageController : PacketControllerBase
    {
        public StorageController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory) { }

        [HttpPost(StorageUpgradeRequest.POST)]
        public async Task<ActionResult<StorageUpgradeResponse>> StorageUpgradeRequestPost([FromBody]StorageUpgradeRequest req)
        {
            StorageUpgradeProcessor processor = new StorageUpgradeProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(SellCropRequest.POST)]
        public async Task<ActionResult<SellCropResponse>> SellCropRequestPost([FromBody]SellCropRequest req)
        {
            SellCropProcessor processor = new SellCropProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}
