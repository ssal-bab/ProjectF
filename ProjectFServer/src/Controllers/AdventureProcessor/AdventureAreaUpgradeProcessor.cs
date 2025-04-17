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
    public class AdventureAreaUpgradeProcessor : PacketProcessorBase<AdventureAreaUpgradeRequest, AdventureAreaUpgradeResponse>
    {
        public AdventureAreaUpgradeProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, AdventureAreaUpgradeRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<AdventureAreaUpgradeResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            userData.adventureData.adventureAreas.TryGetValue(request.areaID, out int level);
            List<AdventureUpgradeCostTableRow> upgradeCostTableRowList = DataTableManager.GetTable<AdventureUpgradeCostTable>().GetRowList(request.areaID, level);
            CheckUpgradeCost<AdventureUpgradeCostTableRow> checkUpgradeCost = new CheckUpgradeCost<AdventureUpgradeCostTableRow>(userData.storageData, upgradeCostTableRowList);
            if(checkUpgradeCost.upgradePossible == false)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            AdventureLevelTableRow levelTableRow = DataTableManager.GetTable<AdventureLevelTable>().GetRow(request.areaID, level);
            if(levelTableRow == null)
                return ErrorPacket(ENetworkResult.DataNotFound);

            if(userData.monetaData.gold < levelTableRow.gold)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.monetaData.gold -= levelTableRow.gold;
                new ApplyUpgradeCost<AdventureUpgradeCostTableRow>(userData.storageData, upgradeCostTableRowList);
                userData.adventureData.adventureAreas[request.areaID] = level + 1;

                await userDataInfo.WriteAsync();
            }

            return new AdventureAreaUpgradeResponse() {
                result = ENetworkResult.Success,
                currentLevel = userData.adventureData.adventureAreas[request.areaID]
            };
        }
    }
}