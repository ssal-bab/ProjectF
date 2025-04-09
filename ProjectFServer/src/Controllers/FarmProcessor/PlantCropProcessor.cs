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

            UserFieldGroupData fieldGroupData = userData.fieldGroupData;
            FieldData fieldData = new GetFieldData(fieldGroupData, request.fieldGroupID, request.fieldID).fieldData;
            if(fieldData == null)
                return ErrorPacket(ENetworkResult.DataNotFound);

            if(fieldData.currentCropID != -1 || fieldData.fieldState != EFieldState.Empty)
                return ErrorPacket(ENetworkResult.InvalidAccess);

            UserSeedPocketData seedPocketData = userData.seedPocketData;
            if(seedPocketData.cropQueue.Count <= 0)
                return ErrorPacket(ENetworkResult.DataNotEnough);

            CropQueueSlot cropQueueSlot = seedPocketData.cropQueue[0];
            int cropID = cropQueueSlot.cropID;

            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                new ApplyPlantCrop(fieldGroupData, request.fieldGroupID, request.fieldID, cropID);

                cropQueueSlot.count -= 1;
                if(cropQueueSlot.count <= 0)
                    seedPocketData.cropQueue.RemoveAt(0);

                new UpdateAllQuestDataProgress(userData, EActionType.PlantSeed, DataDefine.NONE_TARGET, 1);
                new UpdateAllQuestDataProgress(userData, EActionType.PlantTargetSeed, cropID, 1);
                await userDataInfo.WriteAsync();
            }

            return new PlantCropResponse() {
                result = ENetworkResult.Success,
                cropID = cropID
            };
        }
    }
}