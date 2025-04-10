using System;
using System.Collections.Generic;

namespace ProjectF
{
    public static class EnumHelper
    {
        private static readonly Dictionary<Type, Array> enumValuesCache = new Dictionary<Type, Array>();

        public static TEnum[] GetValues<TEnum>() where TEnum : Enum
        {
            Type type = typeof(TEnum);
            if(enumValuesCache.TryGetValue(type, out Array enumValues) == false)
            {
                enumValues = Enum.GetValues(type);
                enumValuesCache.Add(type, enumValues);
            }

            return (TEnum[])enumValues;
        }
    }
}