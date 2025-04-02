using System.Collections.Generic;
using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class ClearRepeatQuestProcessor : PacketProcessorBase<ClearRepeatQuestRequest, ClearRepeatQuestResponse>
    {
        public ClearRepeatQuestProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, ClearRepeatQuestRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<ClearRepeatQuestResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.repeatQuestData.repeatQuestDatas.TryGetValue(request.repeatQuestType, out RepeatQuestData repeatQuestData) == false)
                return ErrorPacket(ENetworkResult.DataNotFound);
                
            RepeatQuestTableRow repeatQuestTableRow = new GetRepeatQuestTableRow(request.repeatQuestType, repeatQuestData.questID).tableRow;
            if(repeatQuestTableRow == null)
                return ErrorPacket(ENetworkResult.DataNotFound);
            
            if(repeatQuestData.currentProgress < new CalculateRepeatQuestTargetValue(repeatQuestTableRow, repeatQuestData.repeatCount).targetValue)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            List<RewardData> reward = new CalculateRepeatQuestReward(repeatQuestTableRow, request.repeatQuestType, userData).rewardDataList;
            if(reward == null)
                return ErrorPacket(ENetworkResult.DataNotFound);

            RepeatQuestTableRow nextRepeatQuestTableRow = GetNextRepeatQuestTableRow(request.repeatQuestType, repeatQuestData.questID);
            if(nextRepeatQuestTableRow == null)
                return ErrorPacket(ENetworkResult.DataNotFound);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                new ApplyReward(userData, reward);

                // id가 0이라면 사이클을 돌았다는 것. repeatCount를 1 늘려준다.
                if(nextRepeatQuestTableRow.id == 0)
                    repeatQuestData.repeatCount += 1;

                repeatQuestData.questID = nextRepeatQuestTableRow.id;
                repeatQuestData.currentProgress = 0;
                repeatQuestData.actionTargetID = new SelectQuestActionTargetID(nextRepeatQuestTableRow.actionType, userData).actionTargetID;
            }

            return new ClearRepeatQuestResponse() {
                result = ENetworkResult.Success,
                repeatQuestType = request.repeatQuestType,
                rewardData = reward,
                newRepeatQuestData = repeatQuestData
            };
        }

        // 다음 퀘스트를 찾는 로직은 다른곳에서 쓸 일이 없기 떄문에 여기다가 만든다.
        private RepeatQuestTableRow GetNextRepeatQuestTableRow(ERepeatQuestType repeatQuestType, int currentID)
        {
            TRow Get<TTable, TRow>(int id) where TRow : RepeatQuestTableRow where TTable : RepeatQuestTable<TRow>
            {
                TRow row = DataTableManager.GetTable<TTable>().GetRow(id + 1);
                if(row == null)
                    return DataTableManager.GetTable<TTable>().GetRow(0);

                return row;
            }

            switch (repeatQuestType)
            {
                case ERepeatQuestType.Crop:
                    return Get<CropRepeatQuestTable, CropRepeatQuestTableRow>(currentID);
                case ERepeatQuestType.Farm:
                    return Get<FarmRepeatQuestTable, FarmRepeatQuestTableRow>(currentID);
                case ERepeatQuestType.Adventure:
                    return Get<AdventureRepeatQuestTable, AdventureRepeatQuestTableRow>(currentID);
            }

            return null;
        }
    }
}