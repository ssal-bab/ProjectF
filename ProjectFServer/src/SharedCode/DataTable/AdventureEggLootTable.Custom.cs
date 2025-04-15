using System.Collections.Generic;
using System.Linq;
using H00N.DataTables;

namespace ProjectF.DataTables
{
    public partial class AdventureEggLootTableRow : DataTableRow
    {
    }

    public partial class AdventureEggLootTable : DataTable<AdventureEggLootTableRow> 
    { 
        public Dictionary<int, Dictionary<int, List<AdventureEggLootTableRow>>> rowListByLevelByID = null;
        public Dictionary<int, Dictionary<int, RatesData>> ratesDataByLevelByID = null;

        protected override void OnTableCreated()
        {
            base.OnTableCreated();

            rowListByLevelByID = new Dictionary<int, Dictionary<int, List<AdventureEggLootTableRow>>>();
            foreach(AdventureEggLootTableRow tableRow in this)
            {
                if(rowListByLevelByID.TryGetValue(tableRow.adventureAreaID, out Dictionary<int, List<AdventureEggLootTableRow>> rowListByLevel) == false)
                {
                    rowListByLevel = new Dictionary<int, List<AdventureEggLootTableRow>>();
                    rowListByLevelByID.Add(tableRow.adventureAreaID, rowListByLevel);
                }

                if(rowListByLevel.TryGetValue(tableRow.adventureAreaLevel, out List<AdventureEggLootTableRow> list) == false)
                {
                    list = new List<AdventureEggLootTableRow>();
                    rowListByLevel.Add(tableRow.adventureAreaLevel, list);
                }

                list.Add(tableRow);
            }  

            ratesDataByLevelByID = new Dictionary<int, Dictionary<int, RatesData>>();
            foreach(var id in rowListByLevelByID.Keys)
            {
                if(ratesDataByLevelByID.TryGetValue(id, out var ratesDataByLevel) == false)
                {
                    ratesDataByLevel = new Dictionary<int, RatesData>();
                    ratesDataByLevelByID.Add(id, ratesDataByLevel);
                }

                Dictionary<int, List<AdventureEggLootTableRow>> rowListByLevel = rowListByLevelByID[id];
                foreach(var level in rowListByLevel.Keys)
                {
                    List<AdventureEggLootTableRow> rowList = rowListByLevel[level];
                    float totalRate = 0f;
                    float[] rates = rowList.Select(i => { 
                        totalRate += i.rate; 
                        return i.rate;
                    }).ToArray();

                    ratesDataByLevel[level] = new RatesData(rates, totalRate);
                }
            }
        }

        public List<AdventureEggLootTableRow> GetRowList(int adventureAreaID, int adventureAreaLevel)
        {
            if(rowListByLevelByID.TryGetValue(adventureAreaID, out Dictionary<int, List<AdventureEggLootTableRow>> rowListByLevel) == false)
                return null;

            rowListByLevel.TryGetValue(adventureAreaLevel, out List<AdventureEggLootTableRow> list);
            return list;
        }

        public RatesData GetRatesData(int adventureAreaID, int adventureAreaLevel)
        {
            if(ratesDataByLevelByID.TryGetValue(adventureAreaID, out Dictionary<int, RatesData> ratesDataByLevel) == false)
                return null;

            ratesDataByLevel.TryGetValue(adventureAreaLevel, out RatesData list);
            return list;
        }
    }
}
