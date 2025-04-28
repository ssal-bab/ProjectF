using System;
using System.Collections.Generic;

namespace ProjectF.Datas
{
    public class UserAdventureDataChecker
    {
        public UserAdventureDataChecker(UserData userData)
        {
            UserAdventureData adventureData = userData.adventureData ??= new UserAdventureData();
            
            adventureData.adventureAreas ??= new Dictionary<int, int>();
            adventureData.adventureFinishDatas ??= new Dictionary<int, DateTime>();

            adventureData.adventureFarmerDatas ??= new Dictionary<string, AdventureFarmerData>();
            foreach(string farmerUUID in adventureData.adventureFarmerDatas.Keys)
            {
                if(userData.farmerData.farmerDatas.ContainsKey(farmerUUID) == false)
                    adventureData.adventureFarmerDatas.Remove(farmerUUID);
            }

            adventureData.adventureRewardDatas ??= new Dictionary<string, AdventureRewardData>();
            foreach(string adventureRewardKey in adventureData.adventureRewardDatas.Keys)
            {
                AdventureRewardData adventureRewardData = adventureData.adventureRewardDatas[adventureRewardKey];
                if(adventureRewardData.rewardList.Count <= 0)
                    adventureData.adventureRewardDatas.Remove(adventureRewardKey);
            }
        }
    }
}