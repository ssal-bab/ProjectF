using System;
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
    public class MakeQuestProcessor : PacketProcessorBase<MakeQuestRequest, MakeQuestResponse>
    {
        public MakeQuestProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, MakeQuestRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<MakeQuestResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            var questTable = DataTableManager.GetTable<QuestTable>();
            var tableRow = questTable[request.questData.id];

            if(tableRow == null)
                return ErrorPacket(ENetworkResult.DataNotFound);
            if(tableRow.questName != request.questData.questName)
                return ErrorPacket(ENetworkResult.Error);
            if(tableRow.questType != request.questData.questType)
                return ErrorPacket(ENetworkResult.Error);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.questData.quests.Add(tableRow.id, request.questData);
            }

            return new MakeQuestResponse() {
                result = ENetworkResult.Success,
                questData = request.questData
            };
        }
    }
}