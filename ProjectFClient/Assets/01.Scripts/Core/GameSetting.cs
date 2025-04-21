using ProjectF.Datas;
using UnityEngine;
using ProjectF.UI.Farms;
using ProjectF.Networks;

namespace ProjectF
{
    public static class GameSetting
    {
        private const string LAST_LOGING_USER_ID_SAVE_KEY = "last_login_user_id_save_key";
        public static string LastLoginUserID {
            get => PlayerPrefs.GetString(LAST_LOGING_USER_ID_SAVE_KEY, DataDefine.NO_USER_ID);
            set => PlayerPrefs.SetString(LAST_LOGING_USER_ID_SAVE_KEY, value);
        }

        private const string SELECTABLE_LOGING_USER_IDS_SAVE_KEY = "selectable_login_user_ids_save_key";
        public static string SelectableLoginUserIDs {
            get => PlayerPrefs.GetString(SELECTABLE_LOGING_USER_IDS_SAVE_KEY, DataDefine.NO_USER_ID);
            set => PlayerPrefs.SetString(SELECTABLE_LOGING_USER_IDS_SAVE_KEY, value);
        }

        private const string FARMER_ORDER_TYPE_SAVE_KEY = "saved_order_type";
        public static EOrderType LastFarmerOrderType {
            get => (EOrderType)PlayerPrefs.GetInt(FARMER_ORDER_TYPE_SAVE_KEY, DataDefine.NO_ORDER_TYPE);
            set => PlayerPrefs.SetInt(FARMER_ORDER_TYPE_SAVE_KEY, (int)value);
        }
        
        private const string FARMER_CLASSIFICATION_TYPE_SAVE_KEY = "saved_classification_type";
        public static EFarmerClassificationType LastFarmerClassificationType {
            get => (EFarmerClassificationType)PlayerPrefs.GetInt(FARMER_CLASSIFICATION_TYPE_SAVE_KEY, DataDefine.NO_CLASSIFICATION_TYPE);
            set => PlayerPrefs.SetInt(FARMER_CLASSIFICATION_TYPE_SAVE_KEY, (int)value);
        }

        private const string LAST_SERVER_CONNECTION_KEY = "last_server_connection";
        public static EServerConnectionType LastServerConnection {
            get => (EServerConnectionType)PlayerPrefs.GetInt(LAST_SERVER_CONNECTION_KEY, (int)EServerConnectionType.Development);
            set => PlayerPrefs.SetInt(LAST_SERVER_CONNECTION_KEY, (int)value);
        }

        public static void ResetGameSetting()
        {
            PlayerPrefs.DeleteKey(LAST_LOGING_USER_ID_SAVE_KEY);
        }
    }
}