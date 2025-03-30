using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectF.Datas;

namespace ProjectF
{
    public struct CalculateFarmerProductivity
    {
        public int value;

        public CalculateFarmerProductivity(EFarmerStatType statType, float stat)
        {
            value = 0;
            
            switch (statType)
            {
                case EFarmerStatType.None:
                    break;
                case EFarmerStatType.MoveSpeed:
                    value = GetSpeedByTargetField(stat);
                    break;
                case EFarmerStatType.Health:
                    value = GetNumberOfCropsHarvested(stat);
                    break;
                case EFarmerStatType.FarmingSkill:
                    value = GetCropsHarvestedAtOnce(stat);
                    break;
                case EFarmerStatType.AdventureSkill:
                    value = GetAdditionalProbabilityInAdventure(stat);
                    break;
                case EFarmerStatType.AdventureHealth:
                    value = GetAdditionalCountInAdventure(stat);
                    break;
            }
        }

        private int GetNumberOfCropsHarvested(float healthStat)
        {
            return (int)Math.Round(healthStat / 100 * 15f, MidpointRounding.AwayFromZero);
        }

        private int GetSpeedByTargetField(float speedStat)
        {
            return (int)Math.Round(speedStat / 100 * 20f, MidpointRounding.AwayFromZero);
        }

        private int GetCropsHarvestedAtOnce(float farmingSkillStat)
        {
            return (int)Math.Round(farmingSkillStat / 100 * 10f, MidpointRounding.AwayFromZero);
        }

        private int GetAdditionalProbabilityInAdventure(float adventureSkillStat)
        {
            return Math.Clamp((int)Math.Round(adventureSkillStat, MidpointRounding.AwayFromZero), 0, 100);
        }

        private int GetAdditionalCountInAdventure(float adventureHealthStat)
        {
            return (int)Math.Round(adventureHealthStat / 100 * 90f, MidpointRounding.AwayFromZero);
        }
    }
}