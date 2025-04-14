using System.Collections.Generic;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct ApplyUpgradeCost<TRow> where TRow : UpgradeCostTableRow
    {
        public bool upgradePossible;

        public ApplyUpgradeCost(UserStorageData storageData, List<TRow> upgradeCostTableRowList)
        {
            upgradePossible = false;
        }
    }
}