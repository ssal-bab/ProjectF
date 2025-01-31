using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using ProjectF.Networks.Packets;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

namespace ProjectF.Networks
{
    public class WebRequest<TResponse> where TResponse : ResponsePacket
    {
        private Connection serverConnection = null;
        private RequestPacket requestPayload = null;
        private Action<TResponse> responseHandler = null;

        public WebRequest(Connection connection, RequestPacket payload, Action<TResponse> handler = null)
        {
            serverConnection = connection;
            requestPayload = payload;
            responseHandler = handler;
        }

        public async UniTask<TResponse> RequestAsync()
        {
            try {
                if(MiddleWare() == false)
                    return null;

                string payloadData = JsonConvert.SerializeObject(requestPayload);
                string url = $"{serverConnection.ConnectionExpression}/{requestPayload.Route}/{requestPayload.Post}";
                using (UnityWebRequest request = Post(url, payloadData, "application/json"))
                {
                    await request.SendWebRequest();

                    Debug.Log($"Response: {request.downloadHandler.text}");
                    if (request.result == Result.Success)
                        return HandleReponse(request.downloadHandler.text);
                    else
                        HandleError(request.error);
                }
            } catch(Exception err) {
                Debug.LogWarning(err);
            }

            return null;
        }

        private bool MiddleWare()
        {
            if(requestPayload == null)
            {
                HandleError("Request Payload is Null");
                return false;
            }

            if(serverConnection == null)
            {
                HandleError("Server Connection is Null");
                return false;
            }

            return true;
        }

        private TResponse HandleReponse(string response)
        {
            TResponse responsePayload = JsonConvert.DeserializeObject<TResponse>(response);
            responseHandler?.Invoke(responsePayload);
            return responsePayload;
        }

        private void HandleError(string error)
        {
            Debug.LogError($"Error: {error}");
        }
    }
}