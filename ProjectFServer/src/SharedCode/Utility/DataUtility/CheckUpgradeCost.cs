using System.Collections.Generic;
using System.Linq;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct CheckUpgradeCost<TRow> where TRow : UpgradeCostTableRow
    {
        public bool upgradePossible;

        public CheckUpgradeCost(UserStorageData storageData, List<TRow> upgradeCostTableRowList)
        {
            upgradePossible = false;

            // 조건이 없는 거다. 즉 업그레이드 가능하다.
            if(upgradeCostTableRowList == null)
            {
                upgradePossible = true;
                return;
            }

            foreach(TRow tableRow in upgradeCostTableRowList)
            {
                if(storageData.cropStorage.TryGetValue(tableRow.costItemID, out Dictionary<ECropGrade, int> cropSlot) == false)
                    return;

                int cropCount = cropSlot.Values.Sum();
                if(cropCount < tableRow.costValue)
                    return;
            }

            upgradePossible = true;
        }
    }
}