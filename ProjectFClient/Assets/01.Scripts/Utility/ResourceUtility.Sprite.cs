using System.Collections.Generic;
using H00N.Resources;
using ProjectF.Dialogues;
using UnityEngine;

namespace ProjectF
{
    public static partial class ResourceUtility
    {
        private static Dictionary<string, Dictionary<int, string>> SpriteNameCache = null;

        private static void InitializeSpriteUtility()
        {
            SpriteNameCache = new Dictionary<string, Dictionary<int, string>>();
        }

        public static Sprite GetNestIcon(int id) => GetSprite("NestIcon", id);
        public static Sprite GetStorageIcon(int id) => GetSprite("StorageIcon", id);
        public static Sprite GetFieldGroupIcon(int id) => GetSprite("FieldGroupIcon", id);
        public static Sprite GetEggIcon(int id) => GetSprite("EggIcon", id);
        public static Sprite GetCropIcon(int id) => GetSprite("CropIcon", id);
        public static Sprite GetMaterialIcon(int id) => GetSprite("MaterialIcon", id);
        public static Sprite GetSeedIcon(int id) => GetSprite("SeedIcon", id);
        public static Sprite GetFarmerIcon(int id) => GetSprite("FarmerIcon", id);
        public static Sprite GetCropGradeIcon(int id) => GetSprite("CropGradeIcon", id);
        public static Sprite GetFarmerStatIcon(int id) => GetSprite("FarmerStatIcon", id);
        public static Sprite GetDialogueSpeakerImage(int id) => GetSprite("DialogueSpeakerImage", id);
        public static Sprite GetAdventureAreaImage(int id) => GetSprite("AdventureAreaImage", id);

        private static Sprite GetSprite(string prefix, int id)
        {
            if (SpriteNameCache.TryGetValue(prefix, out Dictionary<int, string> cache) == false)
            {
                cache = new Dictionary<int, string>();
                SpriteNameCache.Add(prefix, cache);
            }

            if(cache.TryGetValue(id, out string resourceName) == false)
            {
                resourceName = $"{prefix}_{id}";
                cache.Add(id, resourceName);
            }

            return ResourceManager.LoadResource<Sprite>(resourceName);
        }
    }
}