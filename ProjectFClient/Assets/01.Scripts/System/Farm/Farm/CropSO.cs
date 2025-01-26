using H00N.DataTables;
using ProjectF.DataTables;
using UnityEngine;

namespace ProjectF.Farms
{
    [CreateAssetMenu(menuName = "SO/Farm/CropsData")]
    public class CropSO : DataTableSO<CropTable, CropTableRow>
    {
        public Sprite[] cropPlantSprites = null;
    }
}