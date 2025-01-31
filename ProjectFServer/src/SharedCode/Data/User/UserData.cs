namespace ProjectF.Datas
{
    public class UserData
    {
        public string userID = null;
        public UserFarmData farmData = null;

        public UserData(string userID) 
        { 
            this.userID = userID;
            farmData = new UserFarmData();
        }
    }
}