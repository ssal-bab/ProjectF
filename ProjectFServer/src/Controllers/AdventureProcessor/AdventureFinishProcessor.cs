using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectF.Datas;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class AdventureFinishProcessor : PacketProcessorBase<AdventureFinishRequest, AdventureFinishResponse>
    {
        public AdventureFinishProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, AdventureFinishRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<AdventureFinishResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.adventureData.adventureFinishDatas.TryGetValue(request.areaID, out DateTime finishTime) == false)
                return ErrorPacket(ENetworkResult.InvalidAccess);

            if(finishTime > ServerInstance.ServerTime)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            // 보상 수령 가능
            string adventureRewardUUID = Guid.NewGuid().ToString();
            AdventureRewardData adventureRewardData = new AdventureRewardData();
            adventureRewardData.areaID = request.areaID;
            adventureRewardData.farmerList = userData.adventureData.adventureFarmerDatas.Where(i => i.Value.areaID == request.areaID).Select(i => i.Value.farmerUUID).ToList();
            adventureRewardData.rewardList = new Dictionary<int, List<RewardData>>(); // 나중에 채운다

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                foreach(string farmerUUID in adventureRewardData.farmerList)
                    userData.adventureData.adventureFarmerDatas.Remove(farmerUUID);

                userData.adventureData.adventureFinishDatas.Remove(request.areaID);
                userData.adventureData.adventureRewardDatas.Add(adventureRewardUUID, adventureRewardData);

                await userDataInfo.WriteAsync();
            }

            return new AdventureFinishResponse() {
                result = ENetworkResult.Success,
                adventureRewardUUID = adventureRewardUUID,
                rewardData = adventureRewardData,
            };
        }
    }
}