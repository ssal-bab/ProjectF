// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using H00N.DataTables;
// using ProjectF.Datas;
// using ProjectF.DataTables;

// namespace ProjectF
// {
//     public struct GetAdventureResultPack
//     {
//         private const EFarmerStatType ADVENTURE_SKILL = EFarmerStatType.AdventureSkill;
//         private const EFarmerStatType ADVENTURE_HEALTH = EFarmerStatType.AdventureHealth;

//         public AdventureResultPack result;

//         public GetAdventureResultPack(AdventureLootTableRow lootRow, UserData userData)
//         {
//             FarmerStatTable statTable = DataTableManager.GetTable<FarmerStatTable>();

//             var materialLootInfo = new List<RewardData>();
//             var seedLootInfo = new List<RewardData>();

//             var farmerList = userData.farmerData.farmerList.Keys;

//             foreach (var uuid in farmerList)
//             {
//                 var farmerData = userData.farmerData;
//                 var farmer = farmerData.farmerList[uuid];
//                 var statRow = statTable.GetRow(farmer.farmerID);

//                 var statDictionary = new GetFarmerStat(statRow, farmer.level).statDictionary;
//                 int probability = new CalculateFarmerProductivity(ADVENTURE_SKILL, statDictionary[ADVENTURE_SKILL]).value;

//                 Random materialRand = new Random(Guid.NewGuid().GetHashCode());
//                 Random seedRand = new Random(Guid.NewGuid().GetHashCode());

//                 int materialItemCount = lootRow.lootItemCount + (materialRand.Next(2) == 0 ? lootRow.lootItemCountDeviation : -lootRow.lootItemCountDeviation);
//                 int seedItemCount = lootRow.lootSeedCount + (seedRand.Next(2) == 0 ? lootRow.lootItemCountDeviation : -lootRow.lootSeedCountDeviation);

//                 if (materialRand.Next(101) < probability)
//                 {
//                     materialItemCount += new CalculateFarmerProductivity(ADVENTURE_HEALTH, statDictionary[ADVENTURE_HEALTH]).value;
//                 }

//                 if (seedRand.Next(101) < probability)
//                 {
//                     seedItemCount += new CalculateFarmerProductivity(ADVENTURE_HEALTH, statDictionary[ADVENTURE_HEALTH]).value;
//                 }

//                 int materialItemID = new GetRandomLootMaterialByProbalities(lootRow).idValue;
//                 int seedItemID = new GetRandomLootSeedByProbalities(lootRow).idValue;

//                 materialLootInfo.Add(new RewardData(ERewardItemType.Material, materialItemID, materialItemCount));
//                 seedLootInfo.Add(new RewardData(ERewardItemType.Seed, seedItemID, seedItemCount));
//             }

//             result = new AdventureResultPack(materialLootInfo, seedLootInfo);
//         }
//     }
// }