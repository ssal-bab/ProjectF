using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using H00N.DataTables;
using Newtonsoft.Json;
using Org.BouncyCastle.Math.EC.Rfc7748;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;
using System.Linq;

namespace ProjectF.Networks.Controllers
{
    public class ClearQuestProcessor : PacketProcessorBase<ClearQuestRequest, ClearQuestResponse>
    {
        public ClearQuestProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, ClearQuestRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<ClearQuestResponse> ProcessInternal()
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
                userData.questData.quests.Remove(tableRow.id);
            }

            return new ClearQuestResponse() {
                result = ENetworkResult.Success,
                questData = request.questData
            };
        }
    }
}