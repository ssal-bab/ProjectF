using ProjectF.Datas;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.DataBases.StoredProcedures;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class LoginProcessor : PacketProcessorBase<LoginRequest, LoginResponse>
    {
        public LoginProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, LoginRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<LoginResponse> ProcessInternal()
        {
            UserData userData = null;
            ENetworkResult result = ENetworkResult.None;

            string userID = request.userID;
            bool twoWayWrite = false;
            if (userID == DataDefine.NO_USER_ID)
            {
                // 새 데이터를 생성한다. 굳이 sql에 저장하진 않는다.
                // 어차피 값의 변화가 일어나면 write할 것이고, 그렇지 않다면 빈 데이터일 테니 저장하지 않아도 된다.
                userID = CreateNewUserID();
                userData = new UserData() { userID = userID };

                result = ENetworkResult.Success;
                twoWayWrite = true;
            }
            else
            {
                // sql에서 데이터를 받아온다
                // sql에도 데이터가 없으면 새로 생성한다
                SearchUserDataByUserIDProcedure procedure = new SearchUserDataByUserIDProcedure(request.userID);
                await procedure.CallAsync();

                if (procedure.Result == ENetworkResult.Success)
                {
                    userData = procedure.UserData;

                    result = ENetworkResult.Success;
                    twoWayWrite = false;
                }
                else if (procedure.Result == ENetworkResult.DataNotFound)
                {
                    userID = CreateNewUserID();
                    userData = new UserData() { userID = userID };

                    result = ENetworkResult.Success;
                    twoWayWrite = true;
                }
                else if (procedure.Result == ENetworkResult.DBError)
                {
                    userData = null;
                    result = ENetworkResult.Error;
                }
            }

            if(result == ENetworkResult.Success)
            {
                UserDataInfo userDataInfo = new UserDataInfo(dbManager.CurrentDB, userID);
                userDataInfo.Data = userData;

                using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
                {
                    new UserDataChecker(userData);
                    await userDataInfo.WriteAsync(twoWayWrite);
                    if(userDataInfo.Result != ENetworkResult.Success)
                    {
                        return new LoginResponse() {
                            result = userDataInfo.Result
                        };
                    }

                    await dbManager.AddUserDataInfoIntoCache(userID, userDataInfo);
                }
            }

            return new LoginResponse() {
                result = result,
                userData = userData,
                serverTime = ServerInstance.ServerTime
            };
        }

        private string CreateNewUserID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}