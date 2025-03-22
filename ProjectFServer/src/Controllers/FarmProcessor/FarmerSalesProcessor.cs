using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectF.Networks.DataBases;
using ProjectFServer.Networks.Packets;
using RedLockNet;
using ProjectF.Datas;
using ProjectF.DataTables;
using H00N.DataTables;

namespace ProjectF.Networks.Controllers
{
    public class FarmerSalesProcessor : PacketProcessorBase<FarmerSalesRequest, FarmerSalesResponse>
    {
        public FarmerSalesProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, FarmerSalesRequest request) : base(dbManager, redLockFactory, request) {  }

        protected override async Task<FarmerSalesResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.farmerData.farmerList.ContainsKey(request.farmerUUID) == false)
                return ErrorPacket(ENetworkResult.DataNotFound);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                userData.monetaData.gold += request.salesAllowance;
                userData.farmerData.farmerList.Remove(request.farmerUUID);
            }

            return new FarmerSalesResponse() {
                result = ENetworkResult.Success,
            };
        }
    }
}