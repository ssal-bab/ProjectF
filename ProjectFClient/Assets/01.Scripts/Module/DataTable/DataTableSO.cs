using UnityEngine;
using Cysharp.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace H00N.DataTables
{
    public class DataTableSO<TTable, TRow> : ScriptableObject where TTable : DataTable<TRow> where TRow : DataTableRow
    {
        public int id = 0;

        private TRow tableRow = null;
        public TRow TableRow { 
            get {
                tableRow ??= GetTableRow();
                return tableRow;
            }
        }

        protected virtual async void OnEnable()
        {
            #if UNITY_EDITOR
            if (EditorApplication.isPlayingOrWillChangePlaymode == false)
                return;
            #endif
        
            if(DataTableManager.Initialized == false)
                await UniTask.WaitUntil(() => DataTableManager.Initialized);

            tableRow = GetTableRow();
            OnTableInitialized();
        }

        protected virtual void OnTableInitialized() {}

        private TRow GetTableRow()
        {
            TTable table = DataTableManager.GetTable<TTable>();
            if (table == null)
                return null;

            return table.GetRow(id);
        }
    }
}