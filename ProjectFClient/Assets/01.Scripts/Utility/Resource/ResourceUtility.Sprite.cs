using System.Collections.Generic;
using H00N.Resources;
using ProjectF.Dialogues;
using UnityEngine;

namespace ProjectF
{
    public static partial class ResourceUtility
    {
        private static Dictionary<string, Dictionary<int, string>> SpriteResourceKeyCache = null;

        private static void InitializeSpriteUtility()
        {
            SpriteResourceKeyCache = new Dictionary<string, Dictionary<int, string>>();
        }

        public const string GOLD_ICON_KEY = "GoldIcon";

        public static string GetNestIconKey(int id) => GetSpriteResourceKey("NestIcon", id);
        public static string GetStorageIconKey(int id) => GetSpriteResourceKey("StorageIcon", id);
        public static string GetFieldGroupIconKey(int id) => GetSpriteResourceKey("FieldGroupIcon", id);
        public static string GetEggIconKey(int id) => GetSpriteResourceKey("EggIcon", id);
        public static string GetCropIconKey(int id) => GetSpriteResourceKey("CropIcon", id);
        // public static string GetMaterialIconKey(int id) => GetSpriteResourceKey("MaterialIcon", id);
        public static string GetSeedIconKey(int id) => GetSpriteResourceKey("SeedIcon", id);
        public static string GetFarmerIconKey(int id) => GetSpriteResourceKey("FarmerIcon", id);
        public static string GetFarmerStatIconKey(int id) => GetSpriteResourceKey("FarmerStatIcon", id);
        public static string GetDialogueSpeakerImageKey(int id) => GetSpriteResourceKey("DialogueSpeakerImage", id);
        public static string GetAdventureAreaImageKey(int id) => GetSpriteResourceKey("AdventureAreaImage", id);
        public static string GetCropGradeIconKey(int id) => id == 0 ? null : GetSpriteResourceKey("CropGradeIcon", id);

        private static string GetSpriteResourceKey(string prefix, int id)
        {
            if (SpriteResourceKeyCache.TryGetValue(prefix, out Dictionary<int, string> cache) == false)
            {
                cache = new Dictionary<int, string>();
                SpriteResourceKeyCache.Add(prefix, cache);
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