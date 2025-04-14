using System.Collections.Generic;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct CheckUpgradeCost<TRow> where TRow : UpgradeCostTableRow
    {
        public bool upgradePossible;

        public CheckUpgradeCost(UserStorageData storageData, List<TRow> upgradeCostTableRowList)
        {
            upgradePossible = false;
        }
    }
}