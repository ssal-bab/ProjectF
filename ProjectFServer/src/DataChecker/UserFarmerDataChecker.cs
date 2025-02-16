namespace ProjectF.Datas
{
    public struct UserFarmerDataChecker
    {
        public UserFarmerDataChecker(UserData userData)
        {
            UserFarmerData farmerData = userData.farmerData ??= new UserFarmerData();
            
            farmerData.farmerList ??= new Dictionary<string, FarmerData>();
        }
    }
}