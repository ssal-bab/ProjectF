using System;
using UnityEngine;

namespace ProjectF.Farms.AI
{
    public class HPConditionDecision : FarmerFSMDecision
    {
        [Flags]
        private enum EConditionType
        {
            None = 0,
            Equal = 1 << 0,
            Less = 1 << 1,
            Greater = 1 << 2,
        }

        [SerializeField, Range(0f, 1f)] float hpThreshold = 0f;
        [SerializeField] EConditionType conditionType = EConditionType.None;

        public override bool MakeDecision()
        {
            if(aiData.CurrentTarget == null)
                return false;

            if(conditionType == EConditionType.None)
                return false;

            float ratio = Farmer.Stat.CurrentHP / Farmer.Stat[Datas.EFarmerStatType.Health];

            if(conditionType.HasFlag(EConditionType.Equal))
            {
                if(ratio == hpThreshold)
                    return true;
            }

            if(conditionType.HasFlag(EConditionType.Less))
            {
                if(ratio < hpThreshold)
                    return true;
            }

            if(conditionType.HasFlag(EConditionType.Greater))
            {
                if(ratio > hpThreshold)
                    return true;
            }

            return false;
        }
    }
}