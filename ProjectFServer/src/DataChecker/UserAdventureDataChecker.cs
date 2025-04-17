using System.Collections.Generic;

namespace ProjectF.Datas
{
    public class UserAdventureDataChecker
    {
        public UserAdventureDataChecker(UserData userData)
        {
            UserAdventureData adventureData = userData.adventureData ??= new UserAdventureData();
            
            adventureData.adventureAreas ??= new Dictionary<int, int>();
            adventureData.adventureProgressDatas ??= new Dictionary<int, AdventureProgressData>();
        }
    }
}