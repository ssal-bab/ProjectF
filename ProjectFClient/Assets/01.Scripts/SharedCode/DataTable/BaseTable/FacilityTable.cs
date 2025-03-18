using System;
using System.Collections.Generic;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    [Serializable]
    public abstract class FacilityTableRow : DataTableRow
    {
        public int level;
        public int upgradeGold;
        public int skipGem;
        public int materialID;
        public int materialCount;
    }

    public class FacilityTable<TRow> : DataTable<TRow> where TRow : FacilityTableRow
    {
        private Dictionary<int, TRow> tableByLevel = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            tableByLevel = new Dictionary<int, TRow>();
            foreach(var tableRow in table.Values)
            {
                if(tableByLevel.ContainsKey(tableRow.level))
                    continue;

                tableByLevel.Add(tableRow.level, tableRow);
            }
        }

        public TRow GetRowByLevel(int level)
        {
            tableByLevel.TryGetValue(level, out TRow tableRow);
            return tableRow;
        }
    }
}