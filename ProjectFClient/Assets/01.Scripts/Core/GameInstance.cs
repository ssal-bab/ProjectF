using System;
using ProjectF.Datas;

namespace ProjectF
{
    public static class GameInstance
    {
        // 프로퍼티로 사용할지, 필드 변수로 사용할지는 좀 더 고민해보자.
        // 어차피 public getter, setter 를 둘 거면 빈번한 접근이 있는 데이터들을 굳이 프로퍼티로 둘 이유가 있을까?

        // private static string currentLoginUserID = "";
        public static string CurrentLoginUserID = null;
        // {
        //     get => currentLoginUserID;
        //     set => currentLoginUserID = value;
        // }

        // private static UserData mainUser = null;
        public static UserData MainUser = null;
        // {
        //     get => mainUser;
        //     set => mainUser = value;
        // }

        // private static DateTime serverTime = new DateTime();
        public static DateTime ServerTime = new DateTime();
        // {
        //     get => serverTime;
        //     set => serverTime = value;
        // }
    }
}