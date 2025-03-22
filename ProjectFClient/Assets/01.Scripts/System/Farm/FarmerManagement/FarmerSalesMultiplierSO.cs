using System;
using System.Collections.Generic;
using H00N.DataTables;
using H00N.Stats;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Farms;
using UnityEngine;

namespace ProjectF.Farms
{
    [CreateAssetMenu(menuName = "SO/Farm/FarmerSalesMultiplier")]
    public class FarmerSalesMultiplierSO : DataTableSO<FarmerSalesTable, FarmerSalesTableRow>
    {
        public float LevelSalesMultiplierValue => TableRow.levelSalesMultiplierValue;
        public float GradeSalesMultiplierValue => TableRow.gradeSalesMultiplierValue;
    }
}
