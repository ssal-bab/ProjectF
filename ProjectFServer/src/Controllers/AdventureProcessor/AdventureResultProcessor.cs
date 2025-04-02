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
        private const EFarmerStatType ADVENTURE_SKILL = EFarmerStatType.AdventureSkill;
        private const EFarmerStatType ADVENTURE_HEALTH = EFarmerStatType.AdventureHealth;

        public AdventureResultProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, AdventureResultRequest request) : base(dbManager, redLockFactory, request)
        {
        }

        protected override async Task<AdventureResultResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.adventureData.inAdventureAreaList.ContainsKey(request.areaID) == false) 
            {
                return ErrorPacket(ENetworkResult.Error);
            }

            AdventureLootTable lootTable = DataTableManager.GetTable<AdventureLootTable>();
            AdventureLootTableRow lootRow = lootTable.GetRow(request.areaID);

            FarmerStatTable statTable = DataTableManager.GetTable<FarmerStatTable>();
            
            var response = new AdventureResultResponse()
            {
                materialLootInfo = new List<AdventureLootMaterialGroup>(),
                seedLootInfo = new List<AdventureLootSeedGroup>(),
                result = ENetworkResult.Success
            };

            foreach (var uuid in userData.adventureData.inExploreFarmerList[request.areaID])
            {
                var farmerData = userData.farmerData;
                var farmer = farmerData.farmerList[uuid];
                var statRow = statTable.GetRow(farmer.farmerID);

                var statDictionary = new GetFarmerStat(statRow, farmer.level).statDictionary;
                int probability = new CalculateFarmerProductivity(ADVENTURE_SKILL, statDictionary[ADVENTURE_SKILL]).value;

                Random materialRand = new Random(Guid.NewGuid().GetHashCode());
                Random seedRand = new Random(Guid.NewGuid().GetHashCode());

                int materialItemCount = lootRow.lootItemCount + (materialRand.Next(2) == 0 ? lootRow.lootItemCountDeviation : -lootRow.lootItemCountDeviation);
                int seedItemCount = lootRow.lootSeedCount + (seedRand.Next(2) == 0 ? lootRow.lootItemCountDeviation : -lootRow.lootSeedCountDeviation);

                if (materialRand.Next(101) < probability)
                {
                    materialItemCount += new CalculateFarmerProductivity(ADVENTURE_HEALTH, statDictionary[ADVENTURE_HEALTH]).value;
                }

                if (seedRand.Next(101) < probability)
                {
                    seedItemCount += new CalculateFarmerProductivity(ADVENTURE_HEALTH, statDictionary[ADVENTURE_HEALTH]).value;
                }

                int materialItemID = new GetRandomLootMaterialByProbalities(lootRow).idValue;
                int seedItemID = new GetRandomLootSeedByProbalities(lootRow).idValue;

                response.materialLootInfo.Add(new AdventureLootMaterialGroup(materialItemID, materialItemCount));
                response.seedLootInfo.Add(new AdventureLootSeedGroup(seedItemID, seedItemCount));

                using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
                {
                    var materialStorage = userData.storageData.materialStorage;

                    if(materialStorage.ContainsKey(materialItemID))
                    {
                        materialStorage[materialItemID] += materialItemCount;
                    }
                    else
                    {
                        materialStorage.Add(materialItemID, materialItemCount);
                    }

                    var seedStorage = userData.seedPocketData.seedStorage;

                    if(seedStorage.ContainsKey(seedItemID))
                    {
                        seedStorage[seedItemID] += seedItemCount;
                    }
                    else
                    {
                        seedStorage.Add(seedItemID, seedItemCount);
                    }

                    userData.adventureData.inAdventureAreaList.Remove(request.areaID);
                    var list = userData.adventureData.inExploreFarmerList[request.areaID];

                    foreach(var farmerUUID in list)
                    {
                        userData.adventureData.allFarmerinExploreList.Remove(farmerUUID);
                    }

                    userData.adventureData.inExploreFarmerList.Remove(request.areaID);

                    new UpdateRepeatQuestProgress(userData, ERepeatQuestType.Adventure, EActionType.AdventureComplete, 1);
                    new UpdateRepeatQuestProgress(userData, ERepeatQuestType.Adventure, EActionType.TargetAdventureComplete, 1, request.areaID);

                    await userDataInfo.WriteAsync();
                }
            }

            return response;
        }
    }
}