using System.Collections.Generic;
using Newtonsoft.Json;

namespace H00N.DataTables
{
    public abstract class DataTable<TRow> : IDataTable where TRow : DataTableRow
    {
        protected Dictionary<int, TRow> table = null;

        public TRow this[int id] => GetRow(id);

        public void CreateTable(string jsonString)
        {
            table = JsonConvert.DeserializeObject<Dictionary<int, TRow>>(jsonString);
            OnTableCreated();
        }

        protected virtual void OnTableCreated() {}

        public TRow GetRow(int id)
        {
            if(table.TryGetValue(id, out TRow row) == false)
                return null;

            return row;
        }
    }
}