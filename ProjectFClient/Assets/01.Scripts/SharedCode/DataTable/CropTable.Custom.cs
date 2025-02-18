using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class CropTableRow : DataTableRow
    {
    }

    public partial class CropTable : DataTable<CropTableRow> 
    {
        private Dictionary<int, CropTableRow> tableByProductID = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            tableByProductID = new Dictionary<int, CropTableRow>();
            foreach(var tableRow in table.Values)
            {
                if(tableByProductID.ContainsKey(tableRow.cropItemID))
                    continue;

                tableByProductID.Add(tableRow.cropItemID, tableRow);
            }
        }

        public CropTableRow GetRowByProductID(int productID)
        {
            tableByProductID.TryGetValue(productID, out CropTableRow tableRow);
            return tableRow;
        }
    }
}