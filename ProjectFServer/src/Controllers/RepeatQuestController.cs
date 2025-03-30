using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [Route(NetworkDefine.REPEAT_QUEST_ROUTE)]
    public class RepeatQuestController : PacketControllerBase
    {
        public RepeatQuestController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory) { }

        [HttpPost(ClearRepeatQuestRequest.POST)]
        public async Task<ActionResult<ClearRepeatQuestResponse>> ClearRepeatQuestRequestPost([FromBody]ClearRepeatQuestRequest req)
        {
            ClearRepeatQuestProcessor processor = new ClearRepeatQuestProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}
