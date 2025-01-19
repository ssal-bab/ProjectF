
namespace ProjectCoin.Networks.Payloads
{
    public class RankingListRequest : RequestPayload
    {
        public override string Route => NetworkDefine.RANKING_ROUTE;

        public const string POST = "ranking_list";
        public override string Post => POST;

        public int index = 0;
        public int count = 0;

        public RankingListRequest(int index, int count)
        {
            this.index = index;
            this.count = count;
        }
    }
}
