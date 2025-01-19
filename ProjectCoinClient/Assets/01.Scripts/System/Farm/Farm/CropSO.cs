using H00N.DataTables;
using ProjectCoin.DataTables;
using UnityEngine;

namespace ProjectCoin.Farms
{
    [CreateAssetMenu(menuName = "SO/Farm/CropsData")]
    public class CropSO : DataTableSO<CropTable, CropTableRow>
    {
        public Sprite[] cropPlantSprites = null;
    }
}