using H00N.DataTables;
using System.Collections.Generic;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public partial class FarmerTableRow : DataTableRow
    {
    }

    public partial class FarmerTable : DataTable<FarmerTableRow> 
    {
        private Dictionary<ERarity, List<FarmerTableRow>> farmerRarityTable = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            farmerRarityTable = new Dictionary<ERarity, List<FarmerTableRow>>();
            foreach(FarmerTableRow tableRow in this)
            {
                if(farmerRarityTable.TryGetValue(tableRow.rarity, out List<FarmerTableRow> farmerRarityTableRow) == false)
                {
                    farmerRarityTableRow = new List<FarmerTableRow>();
                    farmerRarityTable.Add(tableRow.rarity, farmerRarityTableRow);
                }

                farmerRarityTableRow.Add(tableRow);
            }
        }

        public List<FarmerTableRow> GetFarmerList(ERarity rarity)
        {
            farmerRarityTable.TryGetValue(rarity, out List<FarmerTableRow> farmerList);
            return farmerList;
        }
    }
}