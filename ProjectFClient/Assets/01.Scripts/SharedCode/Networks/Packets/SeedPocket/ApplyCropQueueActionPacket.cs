using System.Collections.Generic;
using ProjectF.Datas;

namespace ProjectF.Networks.Packets
{
    public class ApplyCropQueueActionRequest : RequestPacket
    {
        public override string Route => NetworkDefine.SEED_POCKET_ROUTE;

        public const string POST = "ApplyCropQueueAction";
        public override string Post => POST;

        public List<CropQueueActionData> cropQueueActionDataList;

        public ApplyCropQueueActionRequest(List<CropQueueActionData> cropQueueActionDataList)
        {
            this.cropQueueActionDataList = cropQueueActionDataList;
        }
    }

    public class ApplyCropQueueActionResponse : ResponsePacket
    {
        public List<CropQueueActionData> verifiedActionDataList;
    }
}