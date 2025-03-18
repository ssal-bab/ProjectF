using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class HatchEggProcessor : PacketProcessorBase<HatchEggRequest, HatchEggResponse>
    {
        public HatchEggProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, HatchEggRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<HatchEggResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;
            if(userData.nestData.hatchingEggList.Count <= request.eggIndex)
                return ErrorPacket(ENetworkResult.DataNotFound);

            EggHatchingData hatchingData = userData.nestData.hatchingEggList[request.eggIndex];
            EggTableRow eggTableRow = DataTableManager.GetTable<EggTable>().GetRow(hatchingData.eggID);
            if(eggTableRow == null)
                return ErrorPacket(ENetworkResult.DataNotFound);

            TimeSpan elapsedTime = ServerInstance.ServerTime - hatchingData.hatchingStartTime;
            if(elapsedTime.TotalSeconds < eggTableRow.hatchingTime)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            NestTableRow nestTableRow = DataTableManager.GetTable<NestTable>().GetRowByLevel(userData.nestData.level);
            if(userData.farmerData.farmerList.Count >= nestTableRow.farmerStoreLimit)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            // 알 생성
            List<FarmerTableRow> farmerList = DataTableManager.GetTable<FarmerTable>().GetFarmerList(eggTableRow.rarity);
            int index = new Random().Next(0, farmerList.Count);
            FarmerTableRow farmerTableRow = farmerList[index];
            FarmerData farmerData = new GenerateFarmerData(farmerTableRow).farmerData;

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.nestData.hatchingEggList.RemoveAt(request.eggIndex);
                userData.farmerData.farmerList.Add(farmerData.farmerUUID, farmerData);
                await userDataInfo.WriteAsync();
            }

            return new HatchEggResponse() {
                result = ENetworkResult.Success,
                hatchedEggIndex = request.eggIndex,
                farmerData = farmerData
            };
        }
    }
}