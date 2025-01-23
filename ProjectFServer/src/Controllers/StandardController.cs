using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.Payloads;

namespace ProjectF.Networks.Controllers
{
    [ApiController]
    [Route(NetworkDefine.STANDARD_ROUTE)]
    public class StandardController : ControllerBase
    {
        [HttpPost(ServerConnectionRequest.POST)]
        public async Task<ActionResult<ServerConnectionResponse>> ServerConnectionRequestPost([FromBody]RankingListRequest req)
        {
            ServerConnectionResponse response = new ServerConnectionResponse() {
                networkResult = ENetworkResult.Success,
                connection = true,
            };

            return response;
        }
    }
}
