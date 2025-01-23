namespace ProjectF.Datas
{
    public class UserData
    {
        public string userID = null;
        public UserFarmData farmData = null;

        public UserData() 
        { 
            userID = "";
            farmData = new UserFarmData();
        }
    }
}