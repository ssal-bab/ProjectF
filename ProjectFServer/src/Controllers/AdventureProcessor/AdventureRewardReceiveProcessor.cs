using System.Threading.Tasks;
using ProjectF.Datas;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class AdventureRewardReceiveProcessor : PacketProcessorBase<AdventureRewardReceiveRequest, AdventureRewardReceiveResponse>
    {
        public AdventureRewardReceiveProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, AdventureRewardReceiveRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<AdventureRewardReceiveResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.adventureData.adventureRewardDatas.TryGetValue(request.adventureRewardUUID, out AdventureRewardData adventureRewardData) == false)
                return ErrorPacket(ENetworkResult.InvalidAccess);

            if(adventureRewardData.rewardList.ContainsKey(request.index) == false)
                return ErrorPacket(ENetworkResult.InvalidAccess);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                adventureRewardData.rewardList.Remove(request.index);
                if(adventureRewardData.rewardList.Count <= 0)
                    userData.adventureData.adventureRewardDatas.Remove(request.adventureRewardUUID);

                await userDataInfo.WriteAsync();
            }

            return new AdventureRewardReceiveResponse() {
                result = ENetworkResult.Success,
                rewardApplyTime = ServerInstance.ServerTime,
            };
        }
    }
}