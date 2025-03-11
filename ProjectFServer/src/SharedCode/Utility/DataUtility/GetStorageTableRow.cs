using H00N.DataTables;
using ProjectF.DataTables;

namespace ProjectF
{
    public struct GetStorageTableRow
    {
        public StorageTableRow currentStorageTableRow;
        public StorageTableRow nextStorageTableRow;
        public bool isMaxLevel;

        public GetStorageTableRow(int currentLevel)
        {
            StorageTable storageTable = DataTableManager.GetTable<StorageTable>();
            currentStorageTableRow = storageTable.GetRow(currentLevel);
            nextStorageTableRow = storageTable.GetRow(currentLevel + 1);
            isMaxLevel = nextStorageTableRow == null;
        }
    }
}