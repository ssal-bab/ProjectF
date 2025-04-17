using System.Linq;
using System.Threading.Tasks;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;
using ProjectF.Datas;

namespace ProjectF.Networks.Controllers
{
    public class FarmerSalesProcessor : PacketProcessorBase<FarmerSalesRequest, FarmerSalesResponse>
    {
        public FarmerSalesProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, FarmerSalesRequest request) : base(dbManager, redLockFactory, request) {  }

        protected override async Task<FarmerSalesResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            foreach(var uuid in request.farmerUUID)
            {
                if(userData.farmerData.farmerList.ContainsKey(uuid) == false)
                return ErrorPacket(ENetworkResult.DataNotFound);
            }

            var farmerList = userData.farmerData.farmerList;
            var farmerData = farmerList.Keys.Where(farmerList.ContainsKey).Select(k => farmerList[k]);

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                foreach(var data in farmerData)
                {
                    userData.monetaData.gold += new CalculateFarmerSalesAllowance(data.rarity, data.level).value;
                    userData.farmerData.farmerList.Remove(data.farmerUUID);
                }
            }

            return new FarmerSalesResponse() {
                result = ENetworkResult.Success,
            };
        }
    }
}