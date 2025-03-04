namespace ProjectF.Datas
{
    public struct UserMonetaDataChecker
    {
        public UserMonetaDataChecker(UserData userData)
        {
            UserMonetaData monetaData = userData.monetaData ??= new UserMonetaData();
        }
    }
}