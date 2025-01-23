using ProjectCoin.Networks.Payloads;

namespace ProjectCoin.Networks
{
    public class ServerConnection : Connection
    {
        public ServerConnection(string expression) : base(expression) { }

        public override void CheckConnection()
        {
            ServerConnectionRequest payload = new ServerConnectionRequest();
            NetworkManager.Instance.SendWebRequest<ServerConnectionResponse>(this, payload, HandleServerConnectionResponse);
        }

        private void HandleServerConnectionResponse(ServerConnectionResponse response)
        {
            if(response.networkResult != ENetworkResult.Success)
                return;

            SetConnection(response.connection);
        }
    }
}