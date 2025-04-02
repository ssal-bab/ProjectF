using System;
using Cysharp.Threading.Tasks;
using ProjectF.Networks;
using ProjectF.Networks.Packets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectF
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] string baseURL = "https://localhost:7161";
        [SerializeField] string ongoingSceneName = "Intro";

        private async void Start()
        {
            await GameManager.Instance.InitializeAsync();
            await CheckAssetBundleAsync(); // CDN <- AddressableAsset Bundle
            await CheckServerConnection(); // GameServer Connection
            await LogInAuthAsync(); // Google, Apple, Facebook Auth 로그인
            await LogInGameServerAsync(); // UserUID -> GameServer 로그인
        }

        // 에셋 번들 버전 확인
        public async UniTask CheckAssetBundleAsync()
        {

        }

        // 서버 커넥션 확인
        public async UniTask CheckServerConnection()
        {
            baseURL = baseURL.Trim();
            ServerConnection serverConnection = new ServerConnection(baseURL);
            serverConnection.CheckConnection();

            await UniTask.WaitUntil(() => serverConnection.IsConnectionAlive);
            NetworkManager.Instance.SetServetConnection(serverConnection);
        }

        // 소셜 인증
        public async UniTask LogInAuthAsync()
        {
            // 마지막 로그인 정보가 없다면 소셜에 로그인하여 유저 정보를 받아와야 한다
        }

        // 게임 서버 로그인
        public async UniTask LogInGameServerAsync()
        {
            LoginResponse response = await NetworkManager.Instance.SendWebRequestAsync<LoginResponse>(new LoginRequest(), userID: GameSetting.LastLoginUserID);
            if(response.result != ENetworkResult.Success)
            {
                Debug.LogError($"Login rejected. Result : {response.result}");
                return;
            }

            GameInstance.ServerTime = response.serverTime;
            ServerTimeUpdator.Start();

            GameInstance.MainUser = response.userData;
            GameInstance.CurrentLoginUserID = response.userData.userID;
            GameSetting.LastLoginUserID = GameInstance.CurrentLoginUserID;

            SceneManager.LoadScene(ongoingSceneName);
            DateManager.Instance.SetEnable(true);
            GameManager.Instance.OnLoginGameServer();
        }
    }
}
