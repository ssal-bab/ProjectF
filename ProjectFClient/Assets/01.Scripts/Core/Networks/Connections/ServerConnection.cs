using ProjectF.Networks.Packets;

namespace ProjectF.Networks
{
    public class ServerConnection : Connection
    {
        public ServerConnection(string expression) : base(expression) { }

        public override void CheckConnection()
        {
            CheckConnectionInternalAsync();
        }

        private async void CheckConnectionInternalAsync()
        {
            ServerConnectionRequest payload = new ServerConnectionRequest();
            ServerConnectionResponse response = await NetworkManager.Instance.SendWebRequestAsync<ServerConnectionResponse>(this, payload);

            if(response == null || response.result != ENetworkResult.Success)
            {
                SetConnection(false);
                return;
            }

            SetConnection(response.connection);
        }
    }
}