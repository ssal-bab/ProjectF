using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace H00N.FSM
{
    public class FSMBrain : MonoBehaviour
    {
        // <prev, new>
        public UnityEvent<FSMState, FSMState> OnStateChangedEvent = null;

        [Space(15f)]
        [SerializeField] List<FSMParamSO> fsmParams = null;
        private Dictionary<Type, FSMParamSO> fsmParamDictionary = null;

        [Space(15f)]
        [SerializeField] FSMState defaultState = null;
        [SerializeField] FSMState anyState = null;
        private FSMState currentState = null;
        public FSMState CurrentState => currentState;

        private bool isStopped = false;

        public virtual void Initialize()
        {
            fsmParamDictionary = new Dictionary<Type, FSMParamSO>();
            fsmParams.ForEach(i => {
                Type type = i.GetType();
                if (fsmParamDictionary.ContainsKey(type))
                    return;

                fsmParamDictionary.Add(type, Instantiate(i));
            });

            List<FSMState> states = new List<FSMState>();
            transform.GetComponentsInChildren<FSMState>(states);
            states.ForEach(i => i.Init(this));
        }

        protected virtual void Update()
        {
            if(isStopped)
                return;

            if(currentState != null)
                currentState.UpdateState();

            if(anyState != null)
                anyState.UpdateState();
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

        public async UniTask<bool> LockBrainAsync(Func<UniTask> callback)
        {
            if(isStopped)
                return false;

            isStopped = true;
            await callback();
            isStopped = false;
            
            return true;
        }

        public T GetFSMParam<T>() where T : FSMParamSO
        {
            return fsmParamDictionary[typeof(T)] as T;
        }
    }
}