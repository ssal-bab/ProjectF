using System;

namespace ProjectF
{
    public struct GetValueByRates
    {
        public int randomIndex;

        public GetValueByRates(RatesData ratesData)
        {
            if(ratesData == null)
            {
                randomIndex = 0;
                return;
            }

            float delta = 0;
            float randomValue = (float)(new Random().NextDouble() * ratesData.totalRate);
            for (int i = 0; i < ratesData.rates.Length; ++i)
            {
                delta += ratesData.rates[i];
                if(delta >= randomValue)
                    continue;
                
                randomIndex = i;
                return;
            }

            // 여기까지 내려와선 안 됨
            randomIndex = ratesData.rates.Length - 1;
        }
    }
}