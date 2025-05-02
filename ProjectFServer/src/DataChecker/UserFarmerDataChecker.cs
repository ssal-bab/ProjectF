using System.Collections.Generic;

namespace ProjectF.Datas
{
    public struct UserFarmerDataChecker
    {
        public UserFarmerDataChecker(UserData userData)
        {
            UserFarmerData farmerData = userData.farmerData ??= new UserFarmerData();
            
            farmerData.farmerDatas ??= new Dictionary<string, FarmerData>();
            farmerData.farmerMonetaStroage ??= new Dictionary<int, int>();
        }
    }
}