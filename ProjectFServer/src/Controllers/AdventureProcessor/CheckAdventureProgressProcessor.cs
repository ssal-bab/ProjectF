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
    public class CheckAdventureProgressProcessor : PacketProcessorBase<CheckAdventureProgressRequest, CheckAdventureProgressResponse>
    {
        public CheckAdventureProgressProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, CheckAdventureProgressRequest request) : base(dbManager, redLockFactory, request)
        {
        }

        protected override async Task<CheckAdventureProgressResponse> ProcessInternal()
        {
            // UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            // UserData userData = userDataInfo.Data;

            // var adventureData = userData.adventureData;
            // if(!adventureData.inAdventureAreaList.ContainsKey(request.areaID))
            // {
            //     return ErrorPacket(ENetworkResult.Error);
            // }

            // DateTime adventureStartTime = adventureData.inAdventureAreaList[request.areaID];

            // var lootTable = DataTableManager.GetTable<AdventureLootTable>();
            // var lootRow = lootTable.GetRow(request.areaID);

            // var now = DateTime.Now;

            // var difference = now - adventureStartTime;
            // var seconds = difference.TotalSeconds;

            // bool value = seconds >= lootRow.explorationTime;

            return new CheckAdventureProgressResponse() {
                result = ENetworkResult.Success,
                // isCompleteExplore = value,
                // remainTime = lootRow.explorationTime - seconds
            };
        }
    }
}