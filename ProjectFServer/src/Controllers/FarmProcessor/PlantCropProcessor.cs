using System.Threading.Tasks;
using ProjectF.Datas;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class PlantCropProcessor : PacketProcessorBase<PlantCropRequest, PlantCropResponse>
    {
        public PlantCropProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, PlantCropRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<PlantCropResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                new UpdateAllQuestDataProgress(userData, EActionType.PlantSeed, DataDefine.NONE_TARGET, 1);
                new UpdateAllQuestDataProgress(userData, EActionType.PlantTargetSeed, request.cropID, 1);
                await userDataInfo.WriteAsync();
            }

            return new PlantCropResponse() {
                result = ENetworkResult.Success,
                cropID = request.cropID
            };
        }
    }
}