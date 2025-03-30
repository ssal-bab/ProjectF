using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectF.Datas;
using ProjectF.Networks.DataBases;
using ProjectFServer.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class AdventureStartProcessor : PacketProcessorBase<AdventureStartRequest, AdventureStartResponse>
    {
        public AdventureStartProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, AdventureStartRequest request) : base(dbManager, redLockFactory, request)
        {
        }

        protected override async Task<AdventureStartResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            var adventureData = userData.adventureData;

            if(adventureData.inAdventureAreaList.ContainsKey(request.areaID))
            {
                return ErrorPacket(ENetworkResult.Error);
            }

            var farmerData = userData.farmerData;

            if(farmerData.farmerList.Values.Any(f => !request.exploreFarmerList.Contains(f.farmerUUID)))
            {
                return ErrorPacket(ENetworkResult.DataNotFound);
            }

            if(adventureData.allFarmerinExploreList.Any(f => request.exploreFarmerList.Contains(f)))
            {
                return ErrorPacket(ENetworkResult.DataNotFound);
            }

            DateTime now = DateTime.Now;

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                adventureData.inAdventureAreaList.Add(request.areaID, now);
                adventureData.inExploreFarmerList.Add(request.areaID, request.exploreFarmerList);

                foreach(var uuid in request.exploreFarmerList)
                {
                    adventureData.allFarmerinExploreList.Add(uuid);
                }
            }

            return new AdventureStartResponse() {
                result = ENetworkResult.Success,
                adventureStartTime = now
            };
        }
    }
}