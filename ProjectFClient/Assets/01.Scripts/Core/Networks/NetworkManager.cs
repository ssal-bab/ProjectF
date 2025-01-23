using System;
using ProjectCoin.Networks.Payloads;

namespace ProjectCoin.Networks
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

        public void SendWebRequest<TResponse>(RequestPayload payload, Action<TResponse> handler) where TResponse : ResponsePayload
        {
            payload.userID = "";
            new WebRequest<TResponse>(serverConnection, payload, handler).RequestAsync();
        }

        public void SendWebRequest<TResponse>(Connection connection, RequestPayload payload, Action<TResponse> handler) where TResponse : ResponsePayload
        {
            payload.userID = "";
            new WebRequest<TResponse>(connection, payload, handler).RequestAsync();
        }
    }
}