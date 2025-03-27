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
    public class CheatProcessor : PacketProcessorBase<CheatRequest, CheatResponse>
    {
        public CheatProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, CheatRequest request) : base(dbManager, redLockFactory, request) { }
        
        private static readonly Dictionary<string, Func<DBManager, IDistributedLockFactory, CheatRequest, Task<string>>> cheatTable = new Dictionary<string, Func<DBManager, IDistributedLockFactory, CheatRequest, Task<string>>>() {
            ["AddEgg"] = ProcessCommand_AddEgg,
            ["ModifyGold"] = ProcessCommand_ModifyGold
        };

        protected override async Task<CheatResponse> ProcessInternal()
        {
            if(cheatTable.TryGetValue(request.command, out var cheatProcess) == false)
                return ErrorPacket(ENetworkResult.DataNotFound);

            string response = await cheatProcess.Invoke(dbManager, redLockFactory, request);
            return new CheatResponse() {
                result = ENetworkResult.Success,
                response = response
            };
        }

        private static async Task<string> ProcessCommand_AddEgg(DBManager dbManager, IDistributedLockFactory redLockFactory, CheatRequest request)
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;
            NestTableRow nestTableRow = DataTableManager.GetTable<NestTable>().GetRowByLevel(userData.nestData.level);
            if(userData.nestData.hatchingEggList.Count >= nestTableRow.eggStoreLimit)
                return null;

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.nestData.hatchingEggList.Add(new EggHatchingData() {
                    eggID = 0,
                    hatchingStartTime = ServerInstance.ServerTime
                });
                await userDataInfo.WriteAsync();
            }


            return Newtonsoft.Json.JsonConvert.SerializeObject(userData.nestData.hatchingEggList);
        }

        private static async Task<string> ProcessCommand_ModifyGold(DBManager dbManager, IDistributedLockFactory redLockFactory, CheatRequest request)
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;
            int value = int.Parse(request.option[0]);
            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.monetaData.gold += value;
                await userDataInfo.WriteAsync();
            }

            return value.ToString();
        }
    }
}