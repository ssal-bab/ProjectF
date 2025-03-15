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
            ServerConnectionResponse response = await NetworkManager.Instance.SendWebRequestAsync<ServerConnectionResponse>(new ServerConnectionRequest(), this);
            if(response.result != ENetworkResult.Success)
            {
                SetConnection(false);
                return;
            }

            SetConnection(response.connection);
        }
    }
}