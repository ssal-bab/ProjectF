using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectF.Networks.DataBases;
using ProjectFServer.Networks.Packets;
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
            var price = levelupGoldTable.BaseGoldDictionary[farmerRarity] * levelupGoldTable.MultiplierGoldDictionary[farmerRarity] * request.targetLevel;
            int initializedPrice = Convert.ToInt32(Math.Floor(price));

            if(userData.monetaData.gold < initializedPrice)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.monetaData.gold -= initializedPrice;
                userData.farmerData.farmerList[request.farmerUUID].level = request.targetLevel;
            }

            return new FarmerLevelupResponse() {
                result = ENetworkResult.Success,
                farmerUUID = request.farmerUUID,
                targetLevel = request.targetLevel
            };
        }
    }
}