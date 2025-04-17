using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;
using ProjectF.Datas;
using ProjectF.DataTables;
using H00N.DataTables;

namespace ProjectF.Networks.Controllers
{
    public class FarmerLevelupProcessor : PacketProcessorBase<FarmerLevelupRequest, FarmerLevelupResponse>
    {
        public FarmerLevelupProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, FarmerLevelupRequest request) : base(dbManager, redLockFactory, request) {  }

        protected override async Task<FarmerLevelupResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.farmerData.farmerList.ContainsKey(request.farmerUUID) == false)
                return ErrorPacket(ENetworkResult.DataNotFound);

            var levelupGoldTable = DataTableManager.GetTable<FarmerLevelupGoldTable>();
            var farmerRarity = userData.farmerData.farmerList[request.farmerUUID].rarity;
            var level = userData.farmerData.farmerList[request.farmerUUID].level;

            int price = new CalculateFarmerLevelupGold(levelupGoldTable, farmerRarity, level).value;

            if(userData.monetaData.gold < price)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.monetaData.gold -= price;
                userData.farmerData.farmerList[request.farmerUUID].level = level + 1;
            }

            return new FarmerLevelupResponse() {
                result = ENetworkResult.Success,
                farmerUUID = request.farmerUUID
            };
        }
    }
}