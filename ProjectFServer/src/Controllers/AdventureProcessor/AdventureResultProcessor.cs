using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.DataBases;
using ProjectFServer.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class AdventureResultProcessor : PacketProcessorBase<AdventureResultRequest, AdventureResultResponse>
    {
        public AdventureResultProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, AdventureResultRequest request) : base(dbManager, redLockFactory, request)
        {
        }

        protected override async Task<AdventureResultResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if (userData.adventureData.inAdventureAreaList.ContainsKey(request.areaID) == false)
            {
                return ErrorPacket(ENetworkResult.Error);
            }

            AdventureLootTable lootTable = DataTableManager.GetTable<AdventureLootTable>();
            AdventureLootTableRow lootRow = lootTable.GetRow(request.areaID);

            var respones = new AdventureResultResponse
            {
                resultPack = new GetAdventureResultPack(lootRow, userData).result,
                result = ENetworkResult.Success
            };

            return respones;
        }
    }
}