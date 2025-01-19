using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using ProjectCoin.Networks.Payloads;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

namespace ProjectCoin.Networks
{
    public class WebRequest<TResponse> where TResponse : ResponsePayload
    {
        private Connection serverConnection = null;
        private RequestPayload requestPayload = null;
        private Action<TResponse> responseHandler = null;

        public WebRequest(Connection connection, RequestPayload payload, Action<TResponse> handler)
        {
            serverConnection = connection;
            requestPayload = payload;
            responseHandler = handler;
        }

        public async void RequestAsync()
        {
            try {
                if(MiddleWare() == false)
                    return;

                string payloadData = JsonConvert.SerializeObject(requestPayload);
                string url = $"{serverConnection.ConnectionExpression}/{requestPayload.Route}/{requestPayload.Post}";
                using (UnityWebRequest request = Post(url, payloadData, "application/json"))
                {
                    await request.SendWebRequest();

                    Debug.Log($"Response: {request.downloadHandler.text}");
                    if (request.result == Result.Success)
                        HandleReponse(request.downloadHandler.text);
                    else
                        HandleError(request.error);
                }
            } catch(Exception err) {
                Debug.LogWarning(err);
            }
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

        private void HandleReponse(string response)
        {
            TResponse responsePayload = JsonConvert.DeserializeObject<TResponse>(response);
            responseHandler?.Invoke(responsePayload);
        }

        private void HandleError(string error)
        {
            Debug.LogError($"Error: {error}");
        }
    }
}