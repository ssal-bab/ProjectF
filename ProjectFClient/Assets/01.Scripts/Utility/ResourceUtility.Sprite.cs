using System.Collections.Generic;
using H00N.Resources;
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

        public static Sprite GetStorageIcon(int id) => GetSprite("StorageIcon", id);
        public static Sprite GetItemIcon(int id) => GetSprite("ItemIcon", id);
        public static Sprite GetCropGradeIcon(int id) => GetSprite("CropGradeIcon", id);

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