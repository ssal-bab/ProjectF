using System.Threading.Tasks;
using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class HarvestCropProcessor : PacketProcessorBase<HarvestCropRequest, HarvestCropResponse>
    {
        public HarvestCropProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, HarvestCropRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<HarvestCropResponse> ProcessInternal()
        {
            // 나중엔 UserDataInfo 캐싱해야 한다.
            // TwoWayWrite도 매번하는 게 아니라 n분마다 한 번으로 제한해야 한다.
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.fieldGroupData.fieldGroupDatas.TryGetValue(request.fieldGroupID, out FieldGroupData fieldGroupData) == false)
                return ErrorPacket(ENetworkResult.DataNotFound);

            if(fieldGroupData.fieldDatas.TryGetValue(request.fieldID, out FieldData fieldData) == false)
                return ErrorPacket(ENetworkResult.DataNotFound);

            if(userData.farmerData.farmerList.TryGetValue(request.farmerUUID, out FarmerData farmerData) == false)
                return ErrorPacket(ENetworkResult.DataNotFound);

            int productCropID = fieldData.currentCropID;

            FarmerStatTableRow farmerStatTableRow = DataTableManager.GetTable<FarmerStatTable>().GetRow(farmerData.farmerID);
            int cropCount = new CalculateFarmerHarvestCount(farmerStatTableRow, farmerData.level).harvestCount; // 나중엔 cropCount를 request.farmerUUID의 레벨에 따라 다르게 줘야 한다.
            
            FieldGroupTableRow fieldGroupTableRow = DataTableManager.GetTable<FieldGroupTable>().GetRowByLevel(fieldGroupData.level);
            int cropGradeValue = new GetValueByRates(fieldGroupTableRow.rateTable, fieldGroupTableRow.totalRates).randomIndex;
            
            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                fieldData.currentCropID = -1;
                fieldData.currentGrowth = 0;
                fieldData.fieldState = EFieldState.Fallow;
                new UpdateAllQuestDataProgress(userData, EActionType.HarvestCrop, DataDefine.NONE_TARGET, 1);
                new UpdateAllQuestDataProgress(userData, EActionType.HarvestTargetCrop, productCropID, 1);
                await userDataInfo.WriteAsync();
            }
            
            return new HarvestCropResponse() {
                result = ENetworkResult.Success,
                productCropID = productCropID,
                cropGrade = (ECropGrade)cropGradeValue,
                cropCount = cropCount 
            };
        }
    }
}