using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ProjectF
{
    public static class ServerTimeUpdator
    {
        private const int UPDATE_DELAY_MILLISECONDS = 100;
        private static CancellationTokenSource cancellationTokenSource = null;

        public static void Start()
        {
            if(cancellationTokenSource != null)
            {
                Debug.LogWarning("[ServerTimeUpdator::Start] ServerTimeUpdator is already running.");
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();

            _ = UniTask.RunOnThreadPool(UpdateLoop);
        }

        public static void Stop()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }

        private static async void UpdateLoop()
        {
            while(true)
            {
                DateTime lastUpdateTime = DateTime.UtcNow;
                await UniTask.Delay(UPDATE_DELAY_MILLISECONDS, cancellationToken: cancellationTokenSource.Token);
                TimeSpan difference = DateTime.UtcNow - lastUpdateTime;
                GameInstance.ServerTime = GameInstance.ServerTime.Add(difference);
            }
        }
    }
}