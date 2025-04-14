// using H00N.DataTables;
// using ProjectF.DataTables;

// namespace ProjectF
// {
//     public struct GetFacilityTableRow<TTable, TRow> where TRow : FacilityTableRow where TTable : FacilityTable<TRow>
//     {
//         public TRow currentTableRow;
//         public TRow nextTableRow;
//         public bool isMaxLevel;

//         public GetFacilityTableRow(int currentLevel)
//         {
//             TTable table = DataTableManager.GetTable<TTable>();
//             currentTableRow = table.GetRowByLevel(currentLevel);
//             nextTableRow = table.GetRowByLevel(currentLevel + 1);
//             isMaxLevel = nextTableRow == null;
//         }
//     }
// }