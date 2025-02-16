using ProjectF.Datas;
using UnityEngine;

namespace ProjectF
{
    public static class GameSetting
    {
        private const string LAST_LOGING_USER_ID_SAVE_KEY = "last_login_user_id_save_key";
        public static string LastLoginUserID {
            get => PlayerPrefs.GetString(LAST_LOGING_USER_ID_SAVE_KEY, DataDefine.NO_USER_ID);
            set => PlayerPrefs.SetString(LAST_LOGING_USER_ID_SAVE_KEY, value);
        }
    }
}