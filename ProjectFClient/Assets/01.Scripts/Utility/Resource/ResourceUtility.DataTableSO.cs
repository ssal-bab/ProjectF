using System.Collections.Generic;

namespace ProjectF
{
    public static partial class ResourceUtility
    {
        private static Dictionary<string, Dictionary<int, string>> DataTableSOResourceKeyCache = null;

        private static void InitializeDataTableSOUtility()
        {
            DataTableSOResourceKeyCache = new Dictionary<string, Dictionary<int, string>>();
        }

        public static string GetCropSOKey(int id) => GetDataTableSOResourceKey("CropData", id);

        private static string GetDataTableSOResourceKey(string prefix, int id)
        {
            if (DataTableSOResourceKeyCache.TryGetValue(prefix, out Dictionary<int, string> cache) == false)
            {
                cache = new Dictionary<int, string>();
                DataTableSOResourceKeyCache.Add(prefix, cache);
            }

            if(cache.TryGetValue(id, out string resourceKey) == false)
            {
                resourceKey = $"{prefix}_{id}";
                cache.Add(id, resourceKey);
            }

            return resourceKey;
        }
    }
}