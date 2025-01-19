using H00N.DataTables;
using ProjectCoin.DataTables;
using UnityEngine;

namespace ProjectCoin.Farms
{
    [CreateAssetMenu(menuName = "SO/Buildings/CropStorageData")]
    public class CropStorageSO : DataTableSO<CropStorageTable, CropStorageTableRow>
    {
        public Sprite cropStorageSprite = null;
    }
}