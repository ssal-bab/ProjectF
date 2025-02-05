using System.Collections.Generic;
using H00N.Resources;
using UnityEngine;

namespace ProjectF.Farms
{
    public struct GetStorageIcon
    {
        private static Dictionary<int, string> iconNameCache = new Dictionary<int, string>();
        public Sprite sprite;
        
        public GetStorageIcon(int id)
        {
            if (iconNameCache.TryGetValue(id, out string iconName) == false)
            {
                iconName = $"StorageIcon_{id}";
                iconNameCache.Add(id, iconName);
            }

            sprite = ResourceManager.LoadResource<Sprite>(iconName);
        }
    }
}