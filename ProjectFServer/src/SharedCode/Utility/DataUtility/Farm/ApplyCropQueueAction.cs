using System;
using System.Collections.Generic;

namespace ProjectF.Datas
{
    public struct ApplyCropQueueAction
    {
        public ApplyCropQueueAction(UserData userData, List<CropQueueActionData> actionDataList, Action<CropQueueActionData> applySuccessCallback = null)
        {
            UserSeedPocketData seedPocketData = userData.seedPocketData;
            CropQueue cropQueue = new CropQueue(seedPocketData.cropQueue);

            foreach (CropQueueActionData actionData in actionDataList)
            {
                if (actionData.actionType == ECropQueueActionType.Enqueue)
                {
                    if(seedPocketData.seedStorage.TryGetValue(actionData.target, out int count) == false)
                        break;

                    if(count < actionData.count)
                        break;

                    // 적용 가능한 액션이다.
                    cropQueue.EnqueueCrop(actionData.target, actionData.count);
                    seedPocketData.seedStorage[actionData.target] = count - actionData.count;
                }
                else if (actionData.actionType == ECropQueueActionType.Remove)
                {
                    if(actionData.target < 0 || actionData.target > cropQueue.Count - 1)
                        break;

                    // 적용 가능한 액션이다.
                    cropQueue.RemoveFromCropQueue(actionData.target, actionData.count);
                    if(seedPocketData.seedStorage.TryGetValue(actionData.target, out int count) == false)
                        count = 0;

                    seedPocketData.seedStorage[actionData.target] = count + actionData.count;
                }
                
                // 무사히 적용 되었다.
                applySuccessCallback?.Invoke(actionData);
            }
        }
    }
}