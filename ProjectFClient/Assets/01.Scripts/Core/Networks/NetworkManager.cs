using System;
using Cysharp.Threading.Tasks;
using ProjectF.Networks.Packets;

namespace ProjectF.Networks
{
    public class NetworkManager
    {
        private static NetworkManager instance = null;
        public static NetworkManager Instance => instance;

        private ServerConnection serverConnection = null;

        public void Initialize()
        {
            instance = this;
        }

        public void Release()
        {
            instance = null;
        }

        public void SetServetConnection(ServerConnection connection)
        {
            serverConnection = connection;
        }

        public async UniTask<TResponse> SendWebRequestAsync<TResponse>(RequestPacket packet) where TResponse : ResponsePacket
        {
            return await SendWebRequestAsync<TResponse>(serverConnection, packet);
        }

        public async UniTask<TResponse> SendWebRequestAsync<TResponse>(Connection connection, RequestPacket packet) where TResponse : ResponsePacket
        {
            packet.userID = GameInstance.CurrentLoginUserID;
            return await new WebRequest<TResponse>(connection, packet).RequestAsync();
        }

        // 우선 callback은 숨겨둔다. 필요할 때 활성화 함.
        // await문 활용하여 response를 핸들링하도록 한다.
        // public async UniTask<TResponse> SendWebRequestAsync<TResponse>(RequestPacket packet, Action<TResponse> handler = null) where TResponse : ResponsePacket
        // {
        //     return await SendWebRequestAsync(serverConnection, packet, handler);
        // }

        // public async UniTask<TResponse> SendWebRequestAsync<TResponse>(Connection connection, RequestPacket packet, Action<TResponse> handler = null) where TResponse : ResponsePacket
        // {
        //     return await new WebRequest<TResponse>(connection, packet, handler).RequestAsync();
        // }
    }
}