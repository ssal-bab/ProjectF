using System.Collections.Generic;
using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct UserSeedsPocketDataChecker
    {
        public UserSeedsPocketDataChecker(UserData userData)
        {
            UserSeedsPocketData storageData = userData.seedsPocketData ??= new UserSeedsPocketData();
            
            storageData.seedsStorage ??= new Dictionary<int, int>();
            CropTable cropTable = DataTableManager.GetTable<CropTable>();
            foreach(CropTableRow tableRow in cropTable)
            {
                if(storageData.seedsStorage.ContainsKey(tableRow.id))
                    continue;

                storageData.seedsStorage.Add(tableRow.id, 0);
            }
        }
    }
}