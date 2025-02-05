namespace ProjectF.Datas
{
    public struct UserCropStorageDataChecker
    {
        public UserCropStorageDataChecker(UserData userData)
        {
            userData.cropStorageData ??= new UserCropStorageData();
            userData.cropStorageData.level = 0;
        }
    }
}