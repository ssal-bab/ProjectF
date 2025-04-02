using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.DataBases;
using ProjectFServer.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class ReceiveAdventureResultProcessor : PacketProcessorBase<ReceiveAdventureResultRequest, ReceiveAdventureResultResponse>
    {
        public ReceiveAdventureResultProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, ReceiveAdventureResultRequest request) : base(dbManager, redLockFactory, request)
        {
        }

        protected async override Task<ReceiveAdventureResultResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if (userData.adventureData.inAdventureAreaList.ContainsKey(request.areaID) == false)
            {
                return ErrorPacket(ENetworkResult.Error);
            }

            AdventureLootTable lootTable = DataTableManager.GetTable<AdventureLootTable>();
            AdventureLootTableRow lootRow = lootTable.GetRow(request.areaID);

            var resultPack = new GetAdventureResultPack(lootRow, userData).result;

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                var materialStorage = userData.storageData.materialStorage;

                foreach (var lootInfo in resultPack.materialLootInfo)
                {
                    var materialItemID = lootInfo.rewardItemID;
                    var materialItemCount = lootInfo.rewardItemAmount;

                    if (materialStorage.ContainsKey(materialItemID))
                    {
                        materialStorage[materialItemID] += materialItemCount;
                    }
                    else
                    {
                        materialStorage.Add(materialItemID, materialItemCount);
                    }
                }

                var seedStorage = userData.seedPocketData.seedStorage;

                foreach (var lootInfo in resultPack.seedLootInfo)
                {
                    var seedItemID = lootInfo.rewardItemID;
                    var seedItemCount = lootInfo.rewardItemAmount;

                    if (seedStorage.ContainsKey(seedItemID))
                    {
                        seedStorage[seedItemID] += seedItemCount;
                    }
                    else
                    {
                        seedStorage.Add(seedItemID, seedItemCount);
                    }
                }

                userData.adventureData.inAdventureAreaList.Remove(request.areaID);
                var list = userData.adventureData.inExploreFarmerList[request.areaID];

                foreach (var farmerUUID in list)
                {
                    userData.adventureData.allFarmerinExploreList.Remove(farmerUUID);
                }

                userData.adventureData.inExploreFarmerList.Remove(request.areaID);
            }

            return new ReceiveAdventureResultResponse
            {
                resultPack = resultPack,
                result = ENetworkResult.Success
            };
        }
    }
}