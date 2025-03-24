using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [Route(NetworkDefine.QUEST_ROUTE)]
    public class QuestController : PacketControllerBase
    {
        public QuestController(DBManager dbManager, IDistributedLockFactory redLockFactory) : base(dbManager, redLockFactory) { }

        [HttpPost(MakeQuestRequest.POST)]
        public async Task<ActionResult<MakeQuestResponse>> MakeQuestRequestPost([FromBody] MakeQuestRequest req)
        {
            MakeQuestProcessor processor = new MakeQuestProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }

        [HttpPost(ClearQuestRequest.POST)]
        public async Task<ActionResult<ClearQuestResponse>> ClearQuestRequestPost([FromBody] ClearQuestRequest req)
        {
            ClearQuestProcessor processor = new ClearQuestProcessor(dbManager, redLockFactory, req);
            await processor.ProcessAsync();
            return processor.Response;
        }
    }
}