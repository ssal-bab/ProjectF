namespace ProjectF.Datas
{
    public struct UserDataChecker
    {
        public UserDataChecker(UserData userData)
        {
            new UserFieldDataChecker(userData);
            new UserStorageDataChecker(userData);
            new UserNestDataChecker(userData);
            new UserFarmerDataChecker(userData);
        }
    }
}