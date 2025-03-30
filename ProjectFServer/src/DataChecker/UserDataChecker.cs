namespace ProjectF.Datas
{
    public struct UserDataChecker
    {
        public UserDataChecker(UserData userData)
        {
            new UserFieldGroupDataChecker(userData);
            new UserStorageDataChecker(userData);
            new UserNestDataChecker(userData);
            new UserFarmerDataChecker(userData);
            new UserMonetaDataChecker(userData);
            new UserSeedsPocketDataChecker(userData);
        }
    }
}