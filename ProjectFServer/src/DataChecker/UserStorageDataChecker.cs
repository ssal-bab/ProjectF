using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct UserStorageDataChecker
    {
        public UserStorageDataChecker(UserData userData)
        {
            UserStorageData storageData = userData.storageData ??= new UserStorageData();
            
            // 0렙으로 시작. 튜토리얼 단계에서 건설하는 것 부터 시작이다.
            // cropStorageData.level = 0;

            // 아직은 튜토리얼이 없으니 1렙부터 시작하도록 해두자.
            if(storageData.level == 0)
                storageData.level = 1;

            storageData.cropStorage ??= new Dictionary<int, Dictionary<ECropGrade, int>>();
            storageData.materialStorage ??= new Dictionary<int, int>();
            foreach(var tableRow in DataTableManager.GetTable<ItemTable>())
            {
                switch(tableRow.itemType)
                {
                    case EItemType.Crop:
                        CheckCropStorage(storageData, tableRow);
                        break;
                    case EItemType.Material:
                        CheckMaterialStorage(storageData, tableRow);
                        break;
                    default:
                        continue;
                }
            }
        }

        private void CheckCropStorage(UserStorageData storageData, ItemTableRow tableRow)
        {
            if(storageData.cropStorage.ContainsKey(tableRow.id))
                return;

            // 총 4단계가 있다. 노별, 똥별, 은별, 금별
            storageData.cropStorage.Add(tableRow.id, new Dictionary<ECropGrade, int>() {
                [ECropGrade.None] = 0,
                [ECropGrade.Bronze] = 0,
                [ECropGrade.Silver] = 0,
                [ECropGrade.Gold] = 0,
            });
        }

        private void CheckMaterialStorage(UserStorageData cropStorageData, ItemTableRow tableRow)
        {
            if(cropStorageData.materialStorage.ContainsKey(tableRow.id))
                return;

            // 총 4단계가 있다. 노별, 똥별, 은별, 금별
            cropStorageData.materialStorage.Add(tableRow.id, 0);
        }
    }
}