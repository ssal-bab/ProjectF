using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectF.Datas;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class ApplyCropQueueActionProcessor : PacketProcessorBase<ApplyCropQueueActionRequest, ApplyCropQueueActionResponse>
    {
        public ApplyCropQueueActionProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, ApplyCropQueueActionRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<ApplyCropQueueActionResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            List<CropQueueActionData> verifiedActionDataList = new List<CropQueueActionData>();
            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                new ApplyCropQueueAction(userData, request.cropQueueActionDataList, verifiedActionDataList.Add);
                await userDataInfo.WriteAsync();
            }

            return new ApplyCropQueueActionResponse() {
                result = ENetworkResult.Success,
                verifiedActionDataList = verifiedActionDataList
            };
        }
    }
}