using ProjectF.Datas;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class UpdateFarmProcessor : PacketProcessorBase<UpdateFarmRequest, UpdateFarmResponse>
    {
        public UpdateFarmProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, UpdateFarmRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<UpdateFarmResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                UserFarmData farmData = userDataInfo.Data.farmData;
                foreach(var dirtiedFieldGroup in request.dirtiedFields)
                {
                    int fieldGroupID = dirtiedFieldGroup.Key;
                    Dictionary<int, FieldData> fieldGroupData = dirtiedFieldGroup.Value;

                    if(fieldGroupData == null)
                        continue;

                    foreach(var dirtiedField in fieldGroupData)
                    {
                        int fieldID = dirtiedField.Key;
                        FieldData fieldData = dirtiedField.Value;

                        farmData.fieldGroupDatas[fieldGroupID].fieldDatas[fieldID] = fieldData;
                    }
                }

                await userDataInfo.WriteAsync();
            }

            return new UpdateFarmResponse();
        }
    }
}