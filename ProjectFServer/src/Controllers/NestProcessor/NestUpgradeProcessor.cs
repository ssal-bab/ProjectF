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
    public class NestUpgradeProcessor : PacketProcessorBase<NestUpgradeRequest, NestUpgradeResponse>
    {
        public NestUpgradeProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, NestUpgradeRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<NestUpgradeResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            List<NestUpgradeCostTableRow> upgradeCostTableRowList = DataTableManager.GetTable<NestUpgradeCostTable>().GetRowListByLevel(userData.nestData.level);
            CheckUpgradeCost<NestUpgradeCostTableRow> checkUpgradeCost = new CheckUpgradeCost<NestUpgradeCostTableRow>(userData.storageData, upgradeCostTableRowList);
            if(checkUpgradeCost.upgradePossible == false)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            NestLevelTableRow levelTableRow = DataTableManager.GetTable<NestLevelTable>().GetRowByLevel(userData.nestData.level);
            if(levelTableRow == null)
                return ErrorPacket(ENetworkResult.DataNotFound);

            if(userData.monetaData.gold < levelTableRow.gold)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.monetaData.gold -= levelTableRow.gold;
                new ApplyUpgradeCost<NestUpgradeCostTableRow>(userData.storageData, upgradeCostTableRowList);
                userData.nestData.level += 1;

                await userDataInfo.WriteAsync();
            }

            return new NestUpgradeResponse() {
                result = ENetworkResult.Success,
                currentLevel = userData.nestData.level
            };
        }
    }
}