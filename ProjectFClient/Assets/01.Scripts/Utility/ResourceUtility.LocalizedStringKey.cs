using System.Collections.Generic;
using ProjectF.Datas;

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
        public static string GetCropGradeNameLocalKey(ECropGrade cropGrade) => GetLocalKey("CropGradeName", (int)cropGrade);
        public static string GetRarityNameLocalKey(ERarity rarity) => GetLocalKey("RarityName", (int)rarity);
        public static string GetQusetDescriptionLocalKey(EQuestType type) => GetLocalKey("QuestType", (int)type);
        public static string GetStatDescriptionLocakKey(EFarmerStatType type) => GetLocalKey("StatType", (int)type);

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