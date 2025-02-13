using H00N.DataTables;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.Farms
{
    [CreateAssetMenu(menuName = "SO/Buildings/CropStorageData")]
    public class CropStorageSO : DataTableSO<StorageTable, StorageTableRow>
    {
        public Sprite cropStorageSprite = null;
    }
}