using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF.Datas
{
    public struct UserCropStorageDataChecker
    {
        public UserCropStorageDataChecker(UserData userData)
        {
            UserCropStorageData cropStorageData = userData.cropStorageData ??= new UserCropStorageData();
            
            // 0렙으로 시작. 튜토리얼 단계에서 건설하는 것 부터 시작이다.
            // cropStorageData.level = 0;

            // 아직은 튜토리얼이 없으니 1렙부터 시작하도록 해두자.
            if(cropStorageData.level == 0)
                cropStorageData.level = 1;

            cropStorageData.cropStorage ??= new Dictionary<int, Dictionary<int, int>>();
            cropStorageData.materialStorage ??= new Dictionary<int, int>();
            foreach(var tableRow in DataTableManager.GetTable<ItemTable>())
            {
                switch(tableRow.itemType)
                {
                    case EItemType.Crop:
                        CheckCropStorage(cropStorageData, tableRow);
                        break;
                    case EItemType.Material:
                        CheckMaterialStorage(cropStorageData, tableRow);
                        break;
                    default:
                        continue;
                }
            }
        }

        private void CheckCropStorage(UserCropStorageData cropStorageData, ItemTableRow tableRow)
        {
            if(cropStorageData.cropStorage.ContainsKey(tableRow.id))
                return;

            // 총 4단계가 있다. 노별, 똥별, 은별, 금별
            cropStorageData.cropStorage.Add(tableRow.id, new Dictionary<int, int>() {
                [0] = 0,
                [1] = 0,
                [2] = 0,
                [3] = 0,
            });
        }

        private void CheckMaterialStorage(UserCropStorageData cropStorageData, ItemTableRow tableRow)
        {
            if(cropStorageData.materialStorage.ContainsKey(tableRow.id))
                return;

            // 총 4단계가 있다. 노별, 똥별, 은별, 금별
            cropStorageData.materialStorage.Add(tableRow.id, 0);
        }
    }
}