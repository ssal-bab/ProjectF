namespace ProjectF.Datas
{
    public struct UserDataChecker
    {
        public UserDataChecker(UserData userData)
        {
            new UserFarmDataChecker(userData);
        }
    }
}