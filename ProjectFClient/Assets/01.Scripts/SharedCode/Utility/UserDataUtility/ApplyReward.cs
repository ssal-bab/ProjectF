using System;
using System.Collections.Generic;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct ApplyReward
    {
        private readonly static Dictionary<ERewardItemType, Func<UserData, RewardData, bool>> checkers = new Dictionary<ERewardItemType, Func<UserData, RewardData, bool>>() {
            [ERewardItemType.Gold] = ApplyGoldChecker,
            [ERewardItemType.FreeGem] = ApplyFreeGemChecker,
            [ERewardItemType.CashGem] = ApplyCashGemChecker,
            [ERewardItemType.Seed] = ApplySeedChecker,
            [ERewardItemType.Material] = ApplyMaterialChecker,
            [ERewardItemType.XP] = ApplyXPChecker,
        };

        private readonly static Dictionary<ERewardItemType, Action<UserData, RewardData>> handlers = new Dictionary<ERewardItemType, Action<UserData, RewardData>>() {
            [ERewardItemType.Gold] = ApplyGold,
            [ERewardItemType.FreeGem] = ApplyFreeGem,
            [ERewardItemType.CashGem] = ApplyCashGem,
            [ERewardItemType.Seed] = ApplySeed,
            [ERewardItemType.Material] = ApplyMaterial,
            [ERewardItemType.XP] = ApplyXP,
        };

        public bool result;

        public ApplyReward(UserData userData, List<RewardData> rewardList)
        {
            result = false;

            foreach(RewardData reward in rewardList)
            {
                // 단 하나라도 수령이 불가능하면 return 한다.
                if(checkers[reward.rewardItemType]?.Invoke(userData, reward) == false)
                    return;
            }

            // 모든 보상이 수령 가능하다면 보상을 수령한다.
            foreach(RewardData reward in rewardList)
                handlers[reward.rewardItemType]?.Invoke(userData, reward);
        }

        private static bool ApplyGoldChecker(UserData userData, RewardData reward) => true;
        private static void ApplyGold(UserData userData, RewardData reward)
        {
            userData.monetaData.gold += reward.rewardItemAmount;
        }

        private static bool ApplyFreeGemChecker(UserData userData, RewardData reward) => true;
        private static void ApplyFreeGem(UserData userData, RewardData reward)
        {
            userData.monetaData.freeGem += reward.rewardItemAmount;
        }

        private static bool ApplyCashGemChecker(UserData userData, RewardData reward) => true;
        private static void ApplyCashGem(UserData userData, RewardData reward)
        {
            userData.monetaData.cashGem += reward.rewardItemAmount;
        }

        private static bool ApplySeedChecker(UserData userData, RewardData reward) => true;
        private static void ApplySeed(UserData userData, RewardData reward)
        {
            if(userData.seedPocketData.seedStorage.ContainsKey(reward.rewardItemID) == false)
                return;

            userData.seedPocketData.seedStorage[reward.rewardItemID] += reward.rewardItemAmount;
        }

        private static bool ApplyMaterialChecker(UserData userData, RewardData reward) => true;
        private static void ApplyMaterial(UserData userData, RewardData reward)
        {
            if(userData.storageData.materialStorage.ContainsKey(reward.rewardItemID) == false)
                return;

            userData.storageData.materialStorage[reward.rewardItemID] += reward.rewardItemAmount;
        }

        private static bool ApplyXPChecker(UserData userData, RewardData reward) => true;
        private static void ApplyXP(UserData userData, RewardData reward)
        {
            // 우선 보류
        }
    }
}