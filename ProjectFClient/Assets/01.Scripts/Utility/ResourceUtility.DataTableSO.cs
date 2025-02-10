using System.Collections.Generic;
using H00N.DataTables;
using H00N.Resources;
using ProjectF.DataTables;
using ProjectF.Farms;

namespace ProjectF
{
    public static partial class ResourceUtility
    {
        private static Dictionary<string, Dictionary<int, string>> DataTableSONameCache = null;

        private static void InitializeDataTableSOUtility()
        {
            DataTableSONameCache = new Dictionary<string, Dictionary<int, string>>();
        }

        public static CropSO GetCropData(int id) => GetDataTableSO<CropSO, CropTable, CropTableRow>("CropData", id);

        private static TSO GetDataTableSO<TSO, TTable, TRow>(string prefix, int id) where TSO : DataTableSO<TTable, TRow> where TTable : DataTable<TRow> where TRow : DataTableRow
        {
            if (DataTableSONameCache.TryGetValue(prefix, out Dictionary<int, string> cache) == false)
            {
                cache = new Dictionary<int, string>();
                DataTableSONameCache.Add(prefix, cache);
            }

            if(cache.TryGetValue(id, out string resourceName) == false)
            {
                resourceName = $"{prefix}_{id}";
                cache.Add(id, resourceName);
            }

            return ResourceManager.LoadResource<TSO>(resourceName);
        }
    }
}