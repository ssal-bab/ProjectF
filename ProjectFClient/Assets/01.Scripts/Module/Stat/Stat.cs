using System;
using UnityEngine;

namespace H00N.Stats
{
    [Serializable]
    public class Stat
    {
        [SerializeField] float baseValue = 10f;

        private float currentValue = 10f;
        public float CurrentValue => currentValue;

        private StatModifier modifiers = new StatModifier();

        public event Action<float> OnValueChangedEvent = null;

        public void Initialize()
        {
            modifiers.Init();
            currentValue = baseValue;
        }

        public void Initialize(float baseValue)
        {
            this.baseValue = currentValue = baseValue;
            modifiers.Init();
        }

        private void CalculateValue()
        {
            currentValue = baseValue;
            modifiers.CalculateValue(ref currentValue);
            OnValueChangedEvent?.Invoke(currentValue);
        }

        public void AddModifier(EStatModifierType modifierType, float value)
        {
            modifiers[modifierType].Add(value);
            CalculateValue();
        }

        public void RemoveModifier(EStatModifierType modifierType, float value)
        {
            modifiers[modifierType].Remove(value);
            CalculateValue();
        }

        public static implicit operator float(Stat left) => left.CurrentValue;
        public static implicit operator int(Stat left) => (int)left.CurrentValue;
    }
}