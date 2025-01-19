using System.Collections.Generic;
using ProjectCoin.Datas;

namespace ProjectCoin.Networks.Payloads
{
    public class RankingListResponse : ResponsePayload
    {
        public List<RankingData> rankingList = null;
    }
}
