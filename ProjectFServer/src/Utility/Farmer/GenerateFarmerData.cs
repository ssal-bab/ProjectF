using System;
using ProjectF.Datas;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct GenerateFarmerData
    {
        public FarmerData farmerData;

        public GenerateFarmerData(FarmerTableRow tableRow)
        {
            farmerData = new FarmerData() {
                farmerUUID = Guid.NewGuid().ToString(),
                farmerID = tableRow.id,
                level = 1,
                rarity = tableRow.rarity,
                nickname = "",
            };
        }
    }
}