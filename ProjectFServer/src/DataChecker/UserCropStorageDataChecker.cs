namespace ProjectF.Datas
{
    public struct UserCropStorageDataChecker
    {
        public UserCropStorageDataChecker(UserData userData)
        {
            userData.cropStorageData ??= new UserCropStorageData();
            
            // 0렙으로 시작. 튜토리얼 단계에서 건설하는 것 부터 시작이다.
            // userData.cropStorageData.level = 0;

            // 아직은 튜토리얼이 없으니 1렙부터 시작하도록 해두자.
            userData.cropStorageData.level = 1;
        }
    }
}