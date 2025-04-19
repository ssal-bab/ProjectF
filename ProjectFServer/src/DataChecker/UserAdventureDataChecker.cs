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
                if(userData.farmerData.farmerList.ContainsKey(farmerUUID) == false)
                    adventureData.adventureFarmerDatas.Remove(farmerUUID);
            }
        }
    }
}