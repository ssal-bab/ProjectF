using System;
using H00N;
using ProjectF.Datas;
using ProjectF.Networks.DataBases.StoredProcedures;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace ProjectF.Networks.DataBases
{
    public class UserDataInfo : RedisDataInfo<UserData>
    {
        public const string TABLE_NAME = "user_key";

        public UserDataInfo(IRedisDatabase db, string userID) : base(db, TABLE_NAME, userID) { }

        protected override async void SideWayWrite()
        {
            // sql 저장
            WriteUserDataProcedure procedure = new WriteUserDataProcedure(Data);
            await procedure.CallAsync();

            if(procedure.Result != ENetworkResult.Success)
                Debug.LogError($"Result : {procedure.Result}\nReason : {procedure.Reason}");
        }
    }
}