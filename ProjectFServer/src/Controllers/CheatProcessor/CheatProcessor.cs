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
            ["ModifyGold"] = ProcessCommand_ModifyGold,
            ["ModifySeed"] = ProcessCommand_ModifySeed
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
            NestLevelTableRow nestTableRow = DataTableManager.GetTable<NestLevelTable>().GetRowByLevel(userData.nestData.level);
            if(userData.nestData.hatchingEggDatas.Count >= nestTableRow.eggStoreLimit)
                return null;

            RewardData rewardData = new RewardData(ERewardItemType.Egg, 0, 1, Guid.NewGuid().ToString());
            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                new ApplyReward(userData, ServerInstance.ServerTime, rewardData);
                await userDataInfo.WriteAsync();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(userData.nestData.hatchingEggDatas);
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

        private static async Task<string> ProcessCommand_ModifySeed(DBManager dbManager, IDistributedLockFactory redLockFactory, CheatRequest request)
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;
            int id = int.Parse(request.option[0]);
            int value = int.Parse(request.option[1]);
            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.seedPocketData.seedStorage[id] += value;
                await userDataInfo.WriteAsync();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new string[] { id.ToString(), value.ToString() });
        }
    }
}