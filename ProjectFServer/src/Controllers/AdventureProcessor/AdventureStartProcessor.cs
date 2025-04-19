using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class AdventureStartProcessor : PacketProcessorBase<AdventureStartRequest, AdventureStartResponse>
    {
        public AdventureStartProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, AdventureStartRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<AdventureStartResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.adventureData.adventureAreas.TryGetValue(request.areaID, out int level) == false)
                return ErrorPacket(ENetworkResult.DataNotFound);

            if(userData.adventureData.adventureFinishDatas.ContainsKey(request.areaID))
                return ErrorPacket(ENetworkResult.InvalidAccess);

            List<AdventureFarmerData> adventureFarmerDataList = new List<AdventureFarmerData>();
            foreach(string farmerUUID in request.farmerList)
            {
                // 이미 탐험을 진행중이면 오류를 반환한다.
                if(userData.farmerData.farmerList.ContainsKey(farmerUUID))
                    return ErrorPacket(ENetworkResult.InvalidAccess);

                // 실제 보유하고 있는 농부인지 확인을 해야하지만. 굳이 여기서 하지는 않는다. 보상을 지급하려 할 때 없는 농부면 제외하고 보상을 주면 된다.
                adventureFarmerDataList.Add(new AdventureFarmerData() {
                    farmerUUID = farmerUUID,
                    areaID = request.areaID
                });
            }

            AdventureLevelTableRow levelTableRow = DataTableManager.GetTable<AdventureLevelTable>().GetRow(request.areaID, level);
            if(levelTableRow == null)
                return ErrorPacket(ENetworkResult.DataNotFound);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.adventureData.adventureFinishDatas.Add(request.areaID, ServerInstance.ServerTime + new TimeSpan(0, 0, 0, levelTableRow.adventureTime));
                foreach(AdventureFarmerData adventureFarmerData in adventureFarmerDataList)
                    userData.adventureData.adventureFarmerDatas.Add(adventureFarmerData.farmerUUID, adventureFarmerData);

                await userDataInfo.WriteAsync();
            }

            return new AdventureStartResponse() {
                result = ENetworkResult.Success,
                finishTime = userData.adventureData.adventureFinishDatas[request.areaID],
            };
        }
    }
}