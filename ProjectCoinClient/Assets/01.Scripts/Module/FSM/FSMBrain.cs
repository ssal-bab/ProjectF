using System;
using System.Collections.Generic;
using H00N.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace H00N.FSM
{
    public class FSMBrain : MonoBehaviour
    {
        // <prev, new>
        public UnityEvent<FSMState, FSMState> OnStateChangedEvent = null;

        [Space(15f)]
        [SerializeField] List<AddressableAsset<FSMParamSO>> fsmParams = null;
        private Dictionary<Type, FSMParamSO> fsmParamDictionary = null;

        [Space(15f)]
        [SerializeField] FSMState defaultState = null;
        private FSMState currentState = null;
        public FSMState CurrentState => currentState;

        public virtual void Initialize()
        {
            fsmParamDictionary = new Dictionary<Type, FSMParamSO>();
            fsmParams.ForEach(i => {
                i.Initialize();
                FSMParamSO paramSO = i.Asset;

                Type type = paramSO.GetType();
                if (fsmParamDictionary.ContainsKey(type))
                    return;

                fsmParamDictionary.Add(type, Instantiate(paramSO));
            });

            List<FSMState> states = new List<FSMState>();
            transform.GetComponentsInChildren<FSMState>(states);
            states.ForEach(i => i.Init(this));
        }

        protected virtual void Update()
        {
            currentState?.UpdateState();
        }

        public void SetAsDefaultState()
        {
            ChangeState(defaultState);
        }

        public void ChangeState(FSMState targetState)
        {
            OnStateChangedEvent?.Invoke(currentState, targetState);

            currentState?.ExitState();
            currentState = targetState;
            currentState?.EnterState();
        }

        public T GetFSMParam<T>() where T : FSMParamSO
        {
            return fsmParamDictionary[typeof(T)] as T;
        }
    }
}