using System;
using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct GetRandomLootSeedByProbalities
    {
        public int idValue;

        public GetRandomLootSeedByProbalities(AdventureLootTableRow row) : this()
        {
            var adConfigTable = DataTableManager.GetTable<AdventureConfigTable>();

            float[] probabilities =
            {
                adConfigTable.LootSeed1Probability,
                adConfigTable.LootSeed2Probability,
                adConfigTable.LootSeed3Probability,
            };

            int idx = 0;
            idx = GetRandomIndex(probabilities);
            
            idValue = 0;
            switch (idx)
            {
                case 0:
                    idValue = row.seed1;
                    break;
                case 1:
                    idValue = row.seed2;
                    break;
                case 2:
                    idValue = row.seed3;
                    break;
            }
        }

        private int GetRandomIndex(float[] probabilities)
        {
            Random rand = new Random();
            float randomValue = (float)rand.NextDouble(); // 0.0 이상 1.0 미만
            float cumulative = 0f;

            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulative += probabilities[i];
                if (randomValue < cumulative)
                    return i;
            }
            return probabilities.Length - 1; // 안전 장치
        }
    }
}