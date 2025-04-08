using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct UserSeedPocketDataChecker
    {
        public UserSeedPocketDataChecker(UserData userData)
        {
            UserSeedPocketData seedData = userData.seedPocketData ??= new UserSeedPocketData();
            
            seedData.seedStorage ??= new Dictionary<int, int>();
            CropTable cropTable = DataTableManager.GetTable<CropTable>();
            foreach(CropTableRow tableRow in cropTable)
            {
                if(seedData.seedStorage.ContainsKey(tableRow.id))
                    continue;

                seedData.seedStorage.Add(tableRow.id, 0);
            }

            // seedData.cropQueue ??= new CropQueue();
            seedData.cropQueue ??= new List<CropQueueSlot>();
        }
    }
}