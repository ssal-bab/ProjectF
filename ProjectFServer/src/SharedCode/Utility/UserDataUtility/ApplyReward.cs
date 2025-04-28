using System;
using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct ApplyReward
    {
        private readonly UserData userData;
        private readonly DateTime time;

        public ApplyReward(UserData userData, DateTime time, List<RewardData> rewardList)
        {
            this.userData = userData;
            this.time = time;

            foreach(RewardData reward in rewardList)
            {
                // 단 하나라도 수령이 불가능하면 return 한다.
                if(CheckRewardInternal(reward) == false)
                    return;
            }

            // 모든 보상이 수령 가능하다면 보상을 수령한다.
            foreach(RewardData reward in rewardList)
                ApplyRewardInternal(reward);
        }

        public ApplyReward(UserData userData, DateTime time, RewardData reward)
        {
            this.userData = userData;
            this.time = time;

            // 단 하나라도 수령이 불가능하면 return 한다.
            if(CheckRewardInternal(reward) == false)
                return;

            // 모든 보상이 수령 가능하다면 보상을 수령한다.
            ApplyRewardInternal(reward);
        }

        private bool CheckRewardInternal(RewardData reward)
        {
            return reward.rewardItemType switch
            {
                ERewardItemType.Gold => ApplyGoldChecker(),
                ERewardItemType.FreeGem => ApplyFreeGemChecker(),
                ERewardItemType.CashGem => ApplyCashGemChecker(),
                ERewardItemType.Seed => ApplySeedChecker(),
                ERewardItemType.XP => ApplyXPChecker(),
                ERewardItemType.Egg => ApplyEggChecker(),
                ERewardItemType.Farmer => ApplyFarmerChecker(),
                _ => false,
            };
        }

        private void ApplyRewardInternal(RewardData reward)
        {
            switch(reward.rewardItemType)
            {
                case ERewardItemType.Gold:
                    ApplyGold(reward);
                    break;
                case ERewardItemType.FreeGem:
                    ApplyFreeGem(reward);
                    break;
                case ERewardItemType.CashGem:
                    ApplyCashGem(reward);
                    break;
                case ERewardItemType.Seed:
                    ApplySeed(reward);
                    break;
                case ERewardItemType.XP:
                    ApplyXP(reward);
                    break;
                case ERewardItemType.Egg:
                    ApplyEgg(reward);
                    break;
                case ERewardItemType.Farmer:
                    ApplyFarmer(reward);
                    break;
                default:
                    break;
            }
        }

        private bool ApplyGoldChecker() => true;
        private void ApplyGold(RewardData reward)
        {
            userData.monetaData.gold += reward.rewardItemAmount;
        }

        private bool ApplyFreeGemChecker() => true;
        private void ApplyFreeGem(RewardData reward)
        {
            userData.monetaData.freeGem += reward.rewardItemAmount;
        }

        private bool ApplyCashGemChecker() => true;
        private void ApplyCashGem(RewardData reward)
        {
            userData.monetaData.cashGem += reward.rewardItemAmount;
        }

        private bool ApplySeedChecker() => true;
        private void ApplySeed(RewardData reward)
        {
            if(userData.seedPocketData.seedStorage.ContainsKey(reward.rewardItemID) == false)
                return;

            userData.seedPocketData.seedStorage[reward.rewardItemID] += reward.rewardItemAmount;
        }

        private bool ApplyXPChecker() => true;
        private void ApplyXP(RewardData reward)
        {
            // 우선 보류
        }

        private bool ApplyEggChecker()
        {
            NestLevelTableRow nestLevelTableRow = DataTableManager.GetTable<NestLevelTable>().GetRowByLevel(userData.nestData.level);
            bool nestFull = userData.nestData.hatchingEggDatas.Count >= nestLevelTableRow.eggStoreLimit;
            return nestFull == false;
        }
        private void ApplyEgg(RewardData reward)
        {
            if(string.IsNullOrEmpty(reward.rewardUUID))
                return;

            EggTableRow eggTableRow = DataTableManager.GetTable<EggTable>().GetRow(reward.rewardItemID);
            if(eggTableRow == null)
                return;

            userData.nestData.hatchingEggDatas.Add(reward.rewardUUID, new EggHatchingData() {
                hatchingFinishTime = time + TimeSpan.FromSeconds(eggTableRow.hatchingTime),
                eggID = reward.rewardItemID,
                eggUUID = reward.rewardUUID,
            });
        }

        private bool ApplyFarmerChecker()
        {
            NestLevelTableRow nestLevelTableRow = DataTableManager.GetTable<NestLevelTable>().GetRowByLevel(userData.nestData.level);
            bool farmerFull = userData.farmerData.farmerDatas.Count >= nestLevelTableRow.farmerStoreLimit;
            return farmerFull == false;
        }
        private void ApplyFarmer(RewardData reward)
        {
            if(string.IsNullOrEmpty(reward.rewardUUID))
                return;

            userData.farmerData.farmerDatas.Add(reward.rewardUUID, new FarmerData() {
                farmerUUID = reward.rewardUUID,
                farmerID = reward.rewardItemID,
                level = 1,
                nickname = "",
            });
        }
    }
}