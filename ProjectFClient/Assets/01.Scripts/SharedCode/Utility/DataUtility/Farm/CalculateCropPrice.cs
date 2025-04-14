using System;
using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct CalculateCropPrice
    {
        public int cropPrice;

        public CalculateCropPrice(int cropID, int cropCount, int storageLevel)
        {
            StorageLevelTableRow storageTableRow = DataTableManager.GetTable<StorageLevelTable>().GetRowByLevel(storageLevel);
            if (storageTableRow == null)
            {
                cropPrice = -1;
                return;
            }

            CropTableRow cropTableRow = DataTableManager.GetTable<CropTable>().GetRow(cropID);
            if (cropTableRow == null)
            {
                cropPrice = -1;
                return;
            }

            cropPrice = (int)Math.Ceiling(cropCount * cropTableRow.basePrice * storageTableRow.priceMultiplier);
        }
    }
}