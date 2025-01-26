using ProjectF.Datas;
using UnityEngine;

namespace ProjectF
{
    public static class GameDefine
    {
        private const string LAST_LOGING_USER_ID_SAVE_KEY = "last_login_user_id_save_key";
        public static string LastLoginUserID {
            get => PlayerPrefs.GetString(LAST_LOGING_USER_ID_SAVE_KEY, DataDefine.NO_USER_ID);
            set => PlayerPrefs.SetString(LAST_LOGING_USER_ID_SAVE_KEY, value);
        }

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