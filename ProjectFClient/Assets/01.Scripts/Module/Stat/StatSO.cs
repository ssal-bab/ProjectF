using System;
using System.Collections.Generic;
using UnityEngine;

namespace H00N.Stats
{
    public abstract class StatSO<TEnum> : ScriptableObject where TEnum : Enum
    {
        [Serializable]
        public class StatSlot
        {
            public TEnum statType;
            public Stat stat;
        }

        public List<StatSlot> stats = new List<StatSlot>();
        private Dictionary<TEnum, Stat> statDictionary;
        public Stat this[TEnum indexer]
        {
            get
            {
                if (statDictionary.ContainsKey(indexer) == false)
                {
                    Debug.LogWarning("Stat of Given Type is Doesn't Existed");
                    return null;
                }

                return statDictionary[indexer];
            }
        }

        public event Action OnStatChangedEvent = null;

        private void OnEnable()
        {
            statDictionary = new Dictionary<TEnum, Stat>();
            stats.ForEach((Action<StatSlot>)(i =>
            {
                if (statDictionary.ContainsKey(i.statType))
                {
                    Debug.LogWarning("Stat of Current Type is Already Existed");
                    return;
                }
                i.stat.Initialize();
                statDictionary.Add(i.statType, i.stat);
            }));
            OnStatChangedEvent?.Invoke();
        }

        public void AddModifier(TEnum statType, EStatModifierType modifierType, float value)
        {
            this[statType]?.AddModifier(modifierType, value);
            OnStatChangedEvent?.Invoke();
        }

        public void RemoveModifier(TEnum statType, EStatModifierType modifierType, float value)
        {
            this[statType]?.RemoveModifier(modifierType, value);
            OnStatChangedEvent?.Invoke();
        }
    }
}