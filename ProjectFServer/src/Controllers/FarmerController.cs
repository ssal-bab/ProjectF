// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using ProjectF.Networks.DataBases;
// using ProjectF.Networks.Packets;
// using RedLockNet;

// namespace ProjectF.Networks.Controllers
// {
//     [ApiController]
//     [Route(NetworkDefine.FARM_ROUTE)]
//     public class FarmerController : PacketControllerBase
//     {
//         public FarmerController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory)
//         {
//         }

//         [HttpPost(FarmerLevelupRequest.POST)]
//         public async Task<ActionResult<FarmerLevelupResponse>> FarmerLevelupRequestPost([FromBody]FarmerLevelupRequest req)
//         {
//             FarmerLevelupProcessor processor = new FarmerLevelupProcessor(dbManager, redLockFactory, req);
//             await processor.ProcessAsync();
//             return processor.Response;
//         }

//         [HttpPost(FarmerSalesRequest.POST)]
//         public async Task<ActionResult<FarmerSalesResponse>> FarmerSalesRequesPost([FromBody]FarmerSalesRequest req)
//         {
//             FarmerSalesProcessor processor = new FarmerSalesProcessor(dbManager, redLockFactory, req);
//             await processor.ProcessAsync();
//             return processor.Response;
//         }
//     }
// }