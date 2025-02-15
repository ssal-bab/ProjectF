using ProjectF.Datas;

namespace ProjectF
{
    public static class GameInstance
    {
        private static string currentLoginUserID = "";
        public static string CurrentLoginUserID {
            get => currentLoginUserID;
            set => currentLoginUserID = value;
        }

        private static UserData mainUser = null;
        public static UserData MainUser {
            get => mainUser;
            set => mainUser = value;
        }
    }
}