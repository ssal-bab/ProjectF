using Microsoft.AspNetCore.Mvc;
using ProjectCoin.Networks.Payloads;
using ProjectCoin.Datas;

namespace ProjectCoin.Networks.Controllers
{
    [ApiController]
    [Route(NetworkDefine.RANKING_ROUTE)]
    public class RankingController : ControllerBase
    {
        [HttpPost(RankingListRequest.POST)]
        public async Task<ActionResult<RankingListResponse>> RankingListRequestPost([FromBody]RankingListRequest req)
        {
            List<RankingData> rankingList = new List<RankingData>();
            // make raking list from redis

            RankingListResponse response = new RankingListResponse() {
                networkResult = ENetworkResult.Success,
                rankingList = rankingList
            };

            return response;
        }
    }
}
