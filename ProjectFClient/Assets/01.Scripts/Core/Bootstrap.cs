using Cysharp.Threading.Tasks;
using ProjectCoin.Networks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectCoin
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] string baseURL = "https://localhost:7161";
        [SerializeField] string ongoingSceneName = "Intro";

        private async void Start()
        {
            await GameManager.Instance.InitializeAsync();
            LogInAsync();
        }

        public async void LogInAsync()
        {
            ServerConnection serverConnection = new ServerConnection(baseURL);
            serverConnection.CheckConnection();

            await UniTask.WaitUntil(() => serverConnection.IsConnectionAlive);
            NetworkManager.Instance.SetServetConnection(serverConnection);

            SceneManager.LoadScene(ongoingSceneName);
            DateManager.Instance.SetEnable(true);
        }
    }
}
