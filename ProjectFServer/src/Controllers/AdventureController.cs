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

        // 탐험 시작 => 종료 시간 설정
        // 탐험 종료 => 보상 설정, 보관함에 보상 추가
        // 탐험 보상 수령 => 보관함에서 보상 삭제, 보상 적용

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