using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class StorageUpgradeProcessor : PacketProcessorBase<StorageUpgradeRequest, StorageUpgradeResponse>
    {
        public StorageUpgradeProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, StorageUpgradeRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<StorageUpgradeResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            GetFacilityTableRow<StorageTable, StorageTableRow> getFacilityTableRow = new GetFacilityTableRow<StorageTable, StorageTableRow>(userData.nestData.level);
            StorageTableRow tableRow = getFacilityTableRow.currentTableRow;
            if(tableRow == null)
                return ErrorPacket(ENetworkResult.Error);

            if(userData.storageData.materialStorage.TryGetValue(tableRow.materialID, out int materialCount) == false)
                return ErrorPacket(ENetworkResult.Error);
            
            if(materialCount < tableRow.materialCount)
                return ErrorPacket(ENetworkResult.DataNotEnough);
             
            if(userData.monetaData.gold < tableRow.materialCount)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.monetaData.gold -= tableRow.materialCount;
                userData.storageData.materialStorage[tableRow.materialID] -= tableRow.materialCount;
                userData.storageData.level += 1;

                await userDataInfo.WriteAsync();
            }

            return new StorageUpgradeResponse() {
                result = ENetworkResult.Success,
                usedGold = tableRow.materialCount,
                usedCostItemID = tableRow.materialID,
                usedCostItemCount = tableRow.materialCount,
                currentLevel = userData.storageData.level
            };
        }
    }
}