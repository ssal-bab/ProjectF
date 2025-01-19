using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace H00N.Stats
{
    [Serializable]
    public class StatModifier
    {
        private Dictionary<EStatModifierType, List<float>> modifiers = null;
        public List<float> this[EStatModifierType indexer] => modifiers[indexer];

        [Header("가수")]
        [SerializeField] List<float> addends = null;

        [Header("합연산 승수")]
        [SerializeField] List<float> sumMultipliers = null;

        [Header("곱연산 승수")]
        [SerializeField] List<float> multiplicationMultipliers = null;

        public void Init()
        {
            addends = new List<float>();
            sumMultipliers = new List<float>();
            multiplicationMultipliers = new List<float>();
            modifiers = new Dictionary<EStatModifierType, List<float>>();

            Type classType = typeof(StatModifier);
            foreach (EStatModifierType modifierType in Enum.GetValues(typeof(EStatModifierType)))
            {
                FieldInfo field = classType.GetField($"{modifierType}s", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field == null)
                    continue;
                modifiers.Add(modifierType, field.GetValue(this) as List<float>);
            }
        }

        public void Clear()
        {
            addends.Clear();
            sumMultipliers.Clear();
            multiplicationMultipliers.Clear();
        }

        public void CalculateValue(ref float inValue)
        {
            CaculateAddends(ref inValue);
            CalculateSumMultipliers(ref inValue);
            CalculateMultiplicationMultipliers(ref inValue);
        }

        private void CaculateAddends(ref float inValue)
        {
            for (int i = 0; i < addends.Count; ++i)
                inValue += addends[i];
        }

        private void CalculateSumMultipliers(ref float inValue)
        {
            float multiplier = 1f;
            for (int i = 0; i < sumMultipliers.Count; ++i)
                multiplier += sumMultipliers[i];
            inValue *= multiplier;
        }

        private void CalculateMultiplicationMultipliers(ref float inValue)
        {
            float multiplier = 1f;
            for (int i = 0; i < multiplicationMultipliers.Count; ++i)
                multiplier *= multiplicationMultipliers[i];
            inValue *= multiplier;
        }
    }
}