using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class AdventureFinishProcessor : PacketProcessorBase<AdventureFinishRequest, AdventureFinishResponse>
    {
        public AdventureFinishProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, AdventureFinishRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<AdventureFinishResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.adventureData.adventureAreas.TryGetValue(request.areaID, out int level) == false)
                return ErrorPacket(ENetworkResult.InvalidAccess);

            if(userData.adventureData.adventureFinishDatas.TryGetValue(request.areaID, out DateTime finishTime) == false)
                return ErrorPacket(ENetworkResult.InvalidAccess);

            if(finishTime > ServerInstance.ServerTime)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            // 보상 수령 가능
            string adventureRewardUUID = Guid.NewGuid().ToString();
            AdventureRewardData adventureRewardData = new AdventureRewardData();
            adventureRewardData.rewardUUID = adventureRewardUUID;
            adventureRewardData.areaID = request.areaID;
            adventureRewardData.farmerList = userData.adventureData.adventureFarmerDatas.Where(i => i.Value.areaID == request.areaID).Select(i => i.Value.farmerUUID).ToList();
            adventureRewardData.rewardList = new Dictionary<int, List<RewardData>>();
            adventureRewardData.rewardList[0] = CalculateCropRewardData(userData, adventureRewardData.farmerList, request.areaID, level);

            int eggCount = new Random().Next(0, DataTableManager.GetTable<GameConfigTable>().AdventureEggLootMaxCount + 1);
            for(int i = 0; i < eggCount; i++)
                adventureRewardData.rewardList[i + 1] = CalculateEggRewardData(request.areaID, level);

            // 이것도 테이블로 빼야함
            RewardData xpReward = new RewardData(ERewardItemType.XP, 0, 1000, null);
            RewardData goldReward = new RewardData(ERewardItemType.Gold, 0, 1000, null);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                new ApplyReward(userData, ServerInstance.ServerTime, xpReward);
                new ApplyReward(userData, ServerInstance.ServerTime, goldReward);
                
                foreach(string farmerUUID in adventureRewardData.farmerList)
                    userData.adventureData.adventureFarmerDatas.Remove(farmerUUID);

                userData.adventureData.adventureFinishDatas.Remove(request.areaID);
                userData.adventureData.adventureRewardDatas.Add(adventureRewardUUID, adventureRewardData);

                await userDataInfo.WriteAsync();
            }

            return new AdventureFinishResponse() {
                result = ENetworkResult.Success,
                xpReward = xpReward,
                goldReward = goldReward,
                adventureRewardUUID = adventureRewardUUID,
                rewardData = adventureRewardData,
                rewardReceiveTime = ServerInstance.ServerTime,
            };
        }

        private static List<RewardData> CalculateCropRewardData(UserData userData, List<string> farmerUUIDList, int areaID, int areaLevel)
        {
            AdventureSkillTable adventureSkillTable = DataTableManager.GetTable<AdventureSkillTable>();

            float cropLootFactor = 0f;
            foreach(string farmerUUID in farmerUUIDList)
            {
                if(userData.farmerData.farmerDatas.TryGetValue(farmerUUID, out FarmerData farmerData) == false)
                    continue;

                FarmerStatTableRow statTableRow = DataTableManager.GetTable<FarmerStatTable>().GetRow(farmerData.farmerID);
                float adventureSkillLevel = new CalculateStat(statTableRow.farmingSkill, farmerData.level).currentStat;
                AdventureSkillTableRow adventureSkillTableRow = adventureSkillTable.GetRowByLevel((int)adventureSkillLevel);

                cropLootFactor += adventureSkillTableRow.additionalLootRate;
            }

            Random random = new Random();
            List<AdventureCropLootTableRow> cropLootTableRowList = DataTableManager.GetTable<AdventureCropLootTable>().GetRowList(areaID, areaLevel);
            List<RewardData> cropRewardData = new List<RewardData>();
            foreach(AdventureCropLootTableRow cropLootTableRow in cropLootTableRowList)
            {
                int cropCount = random.Next(cropLootTableRow.minValue, cropLootTableRow.maxValue + 1);
                cropCount += (int)(cropCount * cropLootFactor);

                cropRewardData.Add(new RewardData(ERewardItemType.Seed, cropLootTableRow.cropID, cropCount, null));
            }

            return cropRewardData;
        }

        private static List<RewardData> CalculateEggRewardData(int areaID, int areaLevel)
        {
            List<AdventureEggLootTableRow> eggLootTableRowList = DataTableManager.GetTable<AdventureEggLootTable>().GetRowList(areaID, areaLevel);
            RatesData eggLootRatesData = DataTableManager.GetTable<AdventureEggLootTable>().GetRatesData(areaID, areaLevel);
            int eggIndex = new GetValueByRates(eggLootRatesData).randomIndex;

            List<RewardData> eggRewardData = new List<RewardData>() {
                new RewardData(ERewardItemType.Egg, eggLootTableRowList[eggIndex].eggID, 1, Guid.NewGuid().ToString()),
            };
            
            return eggRewardData;
        }
    }
}