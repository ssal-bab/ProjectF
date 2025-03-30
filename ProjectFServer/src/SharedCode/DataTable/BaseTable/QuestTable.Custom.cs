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
                tableRow.rewardDataList = new List<RewardData>();
                string[] rewardDataStringList = tableRow.rewardData.Trim().Split(';');
                foreach(string rewardDataString in rewardDataStringList)
                {
                    string trimmedData = rewardDataString.Trim();
                    if (string.IsNullOrEmpty(trimmedData))
                        continue;

                    string[] rewardData = trimmedData.Split(',');
                    ERewardItemType rewardItemType = Enum.Parse<ERewardItemType>(rewardData[0].Trim());
                    int rewardItemID = int.Parse(rewardData[1].Trim());
                    int rewardItemAmount = int.Parse(rewardData[1].Trim());
                    tableRow.rewardDataList.Add(new RewardData(rewardItemType, rewardItemID, rewardItemAmount));
                }
            }
        }
    }
}