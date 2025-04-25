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

            if(userData.nestData.hatchingEggDatas.TryGetValue(request.eggUUID, out EggHatchingData hatchingData) == false)
                return ErrorPacket(ENetworkResult.DataNotFound);

            EggTableRow eggTableRow = DataTableManager.GetTable<EggTable>().GetRow(hatchingData.eggID);
            if(eggTableRow == null)
                return ErrorPacket(ENetworkResult.DataNotFound);

            if(ServerInstance.ServerTime < hatchingData.hatchingFinishTime)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            NestLevelTableRow nestLevelTableRow = DataTableManager.GetTable<NestLevelTable>().GetRowByLevel(userData.nestData.level);
            if(userData.farmerData.farmerDatas.Count >= nestLevelTableRow.farmerStoreLimit)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            // 알 생성
            List<FarmerTableRow> farmerTableRowList = DataTableManager.GetTable<FarmerTable>().GetFarmerList(eggTableRow.rarity);
            int index = new Random().Next(0, farmerTableRowList.Count);
            FarmerTableRow farmerTableRow = farmerTableRowList[index];
            RewardData farmerRewardData = new RewardData(ERewardItemType.Farmer, farmerTableRow.id, 1, Guid.NewGuid().ToString());

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.nestData.hatchingEggDatas.Remove(request.eggUUID);
                new ApplyReward(userData, ServerInstance.ServerTime, new List<RewardData>() { farmerRewardData });
                userData.farmerData.farmerDatas.Add(farmerRewardData.rewardUUID, new FarmerData() {
                    farmerUUID = farmerRewardData.rewardUUID,
                    farmerID = farmerTableRow.id,
                    level = 1,
                    nickname = "",
                });

                new UpdateAllQuestDataProgress(userData, EActionType.HatchEgg, DataDefine.NONE_TARGET, 1);
                new UpdateAllQuestDataProgress(userData, EActionType.HatchTargetEgg, eggTableRow.id, 1);
                await userDataInfo.WriteAsync();
            }

            return new HatchEggResponse() {
                result = ENetworkResult.Success,
                farmerRewardData = farmerRewardData,
                rewardApplyTime = ServerInstance.ServerTime
            };
        }
    }
}