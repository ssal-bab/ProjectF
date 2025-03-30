namespace ProjectF.Datas
{
    public struct UserDataChecker
    {
        public UserDataChecker(UserData userData)
        {
            try {
                new UserFieldGroupDataChecker(userData);
                new UserStorageDataChecker(userData);
                new UserNestDataChecker(userData);
                new UserFarmerDataChecker(userData);
                new UserMonetaDataChecker(userData);
                new UserSeedPocketDataChecker(userData);
                new UserRepeatQuestDataChecker(userData);
            } catch(System.Exception err) 
            {
                System.Console.WriteLine(err);
            }
        }
    }
}