using H00N;

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
                new UserAdventureDataChecker(userData);
            } catch(System.Exception err) 
            {
                Debug.LogError(err);
            }
        }
    }
}