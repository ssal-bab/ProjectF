using System;
using System.Collections;
using System.Collections.Generic;
using ProjectF.Datas;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectF
{
    public static class UserActionObserver
    {
        private static Dictionary<EActionType, Action> OnUserAction;
        private static Dictionary<EActionType, Dictionary<int, Action>> OnUserTargetAction;

        public static void Initialize()
        {
            OnUserAction = new();
            OnUserTargetAction = new();
        }

        public static void RegistObserver(EActionType actionType, Action action)
        {
            if(!OnUserAction.ContainsKey(actionType))
                OnUserAction[actionType] = null;

            OnUserAction[actionType] += action;
        }

        public static void UnregistObserver(EActionType actionType, Action action)
        {
            if(!OnUserAction.ContainsKey(actionType))
                return;

            OnUserAction[actionType] -= action;
        }

        public static void RegistTargetObserver(EActionType actionType, int targetID, Action action)
        {
            if(!OnUserTargetAction.ContainsKey(actionType))
                OnUserTargetAction.Add(actionType, new());

            OnUserTargetAction[actionType].Add(targetID, action);
        }

        public static void UnregistTargetObserver(EActionType actionType, int targetID, Action action)
        {
            if(!OnUserTargetAction.ContainsKey(actionType))
                return;

            OnUserTargetAction[actionType][targetID] -= action;
        }

        public static void Invoke(EActionType actionType)
        {
            if(!OnUserAction.ContainsKey(actionType))
                return;

            OnUserAction[actionType]?.Invoke();
        }

        public static void TargetInvoke(EActionType actionType, int targetID)
        {
            if(!OnUserTargetAction.ContainsKey(actionType))
                return;
            if(OnUserTargetAction[actionType].ContainsKey(targetID))
                return;

            OnUserTargetAction[actionType][targetID]?.Invoke();
        }

        public static void Release()
        {
            OnUserAction.Clear();
            OnUserTargetAction.Clear();
        }
    }
}
