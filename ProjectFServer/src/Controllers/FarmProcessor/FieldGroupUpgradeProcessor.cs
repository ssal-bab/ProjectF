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
    public class FieldGroupUpgradeProcessor : PacketProcessorBase<FieldGroupUpgradeRequest, FieldGroupUpgradeResponse>
    {
        public FieldGroupUpgradeProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, FieldGroupUpgradeRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<FieldGroupUpgradeResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.fieldGroupData.fieldGroupDatas.TryGetValue(request.fieldGroupID, out FieldGroupData fieldGroupData) == false)
                return ErrorPacket(ENetworkResult.Error);

            List<FieldGroupUpgradeCostTableRow> upgradeCostTableRowList = DataTableManager.GetTable<FieldGroupUpgradeCostTable>().GetRowListByLevel(fieldGroupData.level);
            CheckUpgradeCost<FieldGroupUpgradeCostTableRow> checkUpgradeCost = new CheckUpgradeCost<FieldGroupUpgradeCostTableRow>(userData.storageData, upgradeCostTableRowList);
            if(checkUpgradeCost.upgradePossible == false)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            FieldGroupLevelTableRow levelTableRow = DataTableManager.GetTable<FieldGroupLevelTable>().GetRowByLevel(fieldGroupData.level);
            if(levelTableRow == null)
                return ErrorPacket(ENetworkResult.DataNotFound);

            if(userData.monetaData.gold < levelTableRow.gold)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.monetaData.gold -= levelTableRow.gold;
                new ApplyUpgradeCost<FieldGroupUpgradeCostTableRow>(userData.storageData, upgradeCostTableRowList);
                fieldGroupData.level += 1;

                await userDataInfo.WriteAsync();
            }

            return new FieldGroupUpgradeResponse() {
                result = ENetworkResult.Success,
                upgradedFieldGroupID = fieldGroupData.fieldGroupID,
                currentLevel = userData.nestData.level
            };
        }
    }
}