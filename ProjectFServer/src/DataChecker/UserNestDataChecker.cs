using System.Collections.Generic;

namespace ProjectF.Datas
{
    public struct UserNestDataChecker
    {
        public UserNestDataChecker(UserData userData)
        {
            UserNestData nestData = userData.nestData ??= new UserNestData();
            
            // 0렙으로 시작. 튜토리얼 단계에서 건설하는 것 부터 시작이다.
            // nestData.level = 0;

            // 아직은 튜토리얼이 없으니 1렙부터 시작하도록 해두자.
            if(nestData.level == 0)
                nestData.level = 1;

            nestData.hatchingEggDatas ??= new Dictionary<string, EggHatchingData>();
        }
    }
}