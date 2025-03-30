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
            OnUserAction.Add(actionType, action);
        }

        public static void UnregistObserver(EActionType actionType, Action action)
        {
            OnUserAction[actionType] -= action;
        }

        public static void RegistTargetObserver(EActionType actionType, int targetID, Action action)
        {
            if(OnUserTargetAction[actionType] == null)
                OnUserTargetAction[actionType] = new();

            OnUserTargetAction[actionType].Add(targetID, action);
        }

        public static void UnregistTargetObserver(EActionType actionType, int targetID, Action action)
        {
            if(OnUserTargetAction[actionType] == null)
                return;

            OnUserTargetAction[actionType][targetID] -= action;
        }

        public static void Invoke(EActionType actionType)
        {
            OnUserAction[actionType]?.Invoke();
        }

        public static void TargetInvoke(EActionType actionType, int targetID)
        {
            OnUserTargetAction[actionType][targetID]?.Invoke();
        }

        public static void Release()
        {
            OnUserAction.Clear();
            OnUserTargetAction.Clear();
        }
    }
}
