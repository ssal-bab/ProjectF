using System.Collections.Generic;
using H00N.Resources;
using UnityEngine;

namespace ProjectF.Farms
{
    public struct GetCropData
    {
        private static Dictionary<int, string> cropDataNameCache = new Dictionary<int, string>();
        public CropSO currentData;

        public GetCropData(int id)
        {
            if(cropDataNameCache.TryGetValue(id, out string cropDataName) == false)
            {
                cropDataName = $"CropData_{id}";
                cropDataNameCache.Add(id, cropDataName);
            }

            currentData = ResourceManager.LoadResource<CropSO>(cropDataName);
        }
    }
}