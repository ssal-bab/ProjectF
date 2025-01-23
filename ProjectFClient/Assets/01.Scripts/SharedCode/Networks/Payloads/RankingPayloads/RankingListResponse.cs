using System.Collections.Generic;
using ProjectF.Datas;

namespace ProjectF.Networks.Payloads
{
    public class RankingListResponse : ResponsePayload
    {
        public List<RankingData> rankingList = null;
    }
}
