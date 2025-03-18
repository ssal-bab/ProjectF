using System;

namespace ProjectF
{
    public struct GetValueByRates
    {
        public int randomIndex;

        public GetValueByRates(float[] rates, float totalRates)
        {
            float delta = 0;
            float randomValue = (float)(new Random().NextDouble() * totalRates);
            for (int i = 0; i < rates.Length; ++i)
            {
                delta += rates[i];
                if(delta >= randomValue)
                    continue;
                
                randomIndex = i;
                return;
            }

            // 여기까지 내려와선 안 됨
            randomIndex = rates.Length - 1;
        }
    }
}