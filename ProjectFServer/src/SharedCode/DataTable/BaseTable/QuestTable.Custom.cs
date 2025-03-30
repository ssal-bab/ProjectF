using System;
using System.Collections.Generic;
using System.Linq;
using H00N.DataTables;
using ProjectF.Datas;

namespace ProjectF.DataTables
{
    public abstract partial class QuestTableRow : DataTableRow
    {
        public List<RewardData> rewardDataList;
    }

    public abstract partial class QuestTable<TRow> : DataTable<TRow> where TRow : QuestTableRow
    {
        protected override void OnTableCreated()
        {
            base.OnTableCreated();
            foreach(TRow tableRow in this)
            {
                tableRow.rewardDataList = new List<RewardData>(
                    tableRow.rewardData.Split(';').Select(i => {
                        string[] rewardData = i.Split(',');
                        return new RewardData(Enum.Parse<ERewardItemType>(rewardData[0]), int.Parse(rewardData[1]), int.Parse(rewardData[2]));
                    })
                );
            }
        }
    }
}