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
            // 우선은 클라이언트의 데이터를 신뢰하기로 한다.
            // 서버에서 처리하도록 하는 것은 추후에 하자. 지금은 단지 클라이언트가 보낸 데이터를 DB에 저장하기만.

            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                UserFarmData farmData = userDataInfo.Data.farmData;
                foreach(var dirtiedFieldGroup in request.dirtiedFields)
                {
                    Dictionary<int, FieldData> fieldGroupData = dirtiedFieldGroup.Value;
                    FieldGroupData userFieldGroupData = farmData.fieldGroupDatas[dirtiedFieldGroup.Key];

                    if(fieldGroupData == null)
                        continue;

                    foreach(var dirtiedField in fieldGroupData)
                        userFieldGroupData.fieldDatas[dirtiedField.Key].UpdateData(dirtiedField.Value);
                }

                await userDataInfo.WriteAsync();
            }

            return new UpdateFarmResponse();
        }
    }
}