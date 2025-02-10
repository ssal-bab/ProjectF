using System;
using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    [Serializable]
    public class CropTableRow : DataTableRow
    {
        public ECropType cropType;
        public int seedItemID;
        public int growthStep;
        public int growthRate;
        public int productCropID;
        public int basePrice;
        public string nameLocalKey;
    }

    public class CropTable : DataTable<CropTableRow> 
    {
        private Dictionary<int, CropTableRow> tableByProductID = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            tableByProductID = new Dictionary<int, CropTableRow>();
            foreach(var tableRow in table.Values)
            {
                if(tableByProductID.ContainsKey(tableRow.productCropID))
                    continue;

                tableByProductID.Add(tableRow.productCropID, tableRow);
            }
        }

        public CropTableRow GetRowByProductID(int productID)
        {
            tableByProductID.TryGetValue(productID, out CropTableRow tableRow);
            return tableRow;
        }
    }
}