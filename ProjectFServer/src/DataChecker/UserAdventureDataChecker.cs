using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectF.Datas
{
    public class UserAdventureDataChecker
    {
        public UserAdventureDataChecker(UserData userData)
        {
            UserAdventureData adventureData = userData.adventureData ??= new UserAdventureData();
            
            adventureData.adventureList = adventureData.adventureList ??= new();
            adventureData.inAdventureAreaList = adventureData.inAdventureAreaList ??= new();
            adventureData.inExploreFarmerList = adventureData.inExploreFarmerList ??= new();
            adventureData.allFarmerinExploreList = adventureData.allFarmerinExploreList ??= new();
        }
    }
}