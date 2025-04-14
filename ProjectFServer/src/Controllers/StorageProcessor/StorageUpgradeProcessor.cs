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
    public class StorageUpgradeProcessor : PacketProcessorBase<StorageUpgradeRequest, StorageUpgradeResponse>
    {
        public StorageUpgradeProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, StorageUpgradeRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<StorageUpgradeResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            List<StorageUpgradeCostTableRow> upgradeCostTableRowList = DataTableManager.GetTable<StorageUpgradeCostTable>().GetRowListByLevel(userData.storageData.level);
            CheckUpgradeCost<StorageUpgradeCostTableRow> checkUpgradeCost = new CheckUpgradeCost<StorageUpgradeCostTableRow>(userData.storageData, upgradeCostTableRowList);
            if(checkUpgradeCost.upgradePossible == false)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            StorageLevelTableRow levelTableRow = DataTableManager.GetTable<StorageLevelTable>().GetRowByLevel(userData.nestData.level);
            if(levelTableRow == null)
                return ErrorPacket(ENetworkResult.DataNotFound);

            if(userData.monetaData.gold < levelTableRow.gold)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.monetaData.gold -= levelTableRow.gold;
                new ApplyUpgradeCost<StorageUpgradeCostTableRow>(userData.storageData, upgradeCostTableRowList);
                userData.nestData.level += 1;

                await userDataInfo.WriteAsync();
            }

            return new StorageUpgradeResponse() {
                result = ENetworkResult.Success,
                currentLevel = userData.storageData.level
            };
        }
    }
}