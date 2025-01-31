using ProjectF.Networks;
using UnityEngine;

namespace ProjectF.Tests
{
    public class TServer : MonoBehaviour
    {
        private const string BASE_URL = "https://localhost:7161";

        private void Awake()
        {
            new NetworkManager();
        }

        // public void SendRequest()
        // {
        //     RankingListRequest payload = new RankingListRequest(10, 200);
        //     ServerConnection tempConnection = new ServerConnection(BASE_URL);
        //     NetworkManager.Instance.SendWebRequest<RankingListResponse>(
        //         tempConnection, 
        //         payload,
        //         HandleRankingListResponse
        //     );
        // }

        // private void HandleRankingListResponse(RankingListResponse response)
        // {
        //     Debug.Log($"Request Responsed! : {response.networkResult}");
        // }
    }
}