using System.Collections.Generic;

namespace ProjectF
{
    public static partial class ResourceUtility
    {
        private static Dictionary<string, Dictionary<int, string>> LocalizedStringKeyCache = null;

        private static void InitializeLocalizedStringKeyUtility()
        {
            LocalizedStringKeyCache = new Dictionary<string, Dictionary<int, string>>();
        }

        public static string GetCropNameLocalKey(int id) => GetLocalKey("CropName", id);
        public static string GetMaterialNameLocalKey(int id) => GetLocalKey("MaterialName", id);

        private static string GetLocalKey(string prefix, int id)
        {
            if (LocalizedStringKeyCache.TryGetValue(prefix, out Dictionary<int, string> cache) == false)
            {
                cache = new Dictionary<int, string>();
                LocalizedStringKeyCache.Add(prefix, cache);
            }

            if(cache.TryGetValue(id, out string resourceName) == false)
            {
                resourceName = $"{prefix}_{id}";
                cache.Add(id, resourceName);
            }

            return resourceName;
        }
    }
}