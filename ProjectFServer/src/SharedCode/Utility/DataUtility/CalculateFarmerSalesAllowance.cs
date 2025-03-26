using System;
using ProjectF.Datas;
using ProjectF.DataTables;
using H00N.DataTables;

namespace ProjectF
{
    public struct CalculateFarmerSalesAllowance
    {
        public int value;

        public CalculateFarmerSalesAllowance(ERarity rarity, int farmerLevel)
        {
            FarmerConfigTable farmerConfigTable = DataTableManager.GetTable<FarmerConfigTable>();
            float farmingMultiplier = farmerLevel * farmerConfigTable.LevelSalesMultiplierValue;
            float gradeMultiplier = (int)rarity * farmerConfigTable.GradeSalesMultiplierValue;

            value = Convert.ToInt32(MathF.Floor(farmingMultiplier + gradeMultiplier));
        }
    }
}