using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
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
                new UpdateRepeatQuestProgress(userData, ERepeatQuestType.Farm, EActionType.PlantSeed, 1);
                new UpdateRepeatQuestProgress(userData, ERepeatQuestType.Farm, EActionType.PlantTargetSeed, 1, request.cropID);
                await userDataInfo.WriteAsync();
            }

            return new PlantCropResponse() {
                result = ENetworkResult.Success,
                cropID = request.cropID
            };
        }
    }
}