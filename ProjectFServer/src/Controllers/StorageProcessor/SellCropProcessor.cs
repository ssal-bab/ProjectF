using H00N.DataTables;
using ProjectF.Datas;
using ProjectF.DataTables;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class SellCropProcessor : PacketProcessorBase<SellCropRequest, SellCropResponse>
    {
        public SellCropProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, SellCropRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<SellCropResponse> ProcessInternal()
        {
            UserDataInfo userDataInfo = await dbManager.GetUserDataInfo(request.userID);
            UserData userData = userDataInfo.Data;

            if(userData.storageData.cropStorage.TryGetValue(request.id, out Dictionary<ECropGrade, int> cropSlot) == false)
                return ErrorPacket(ENetworkResult.Error);

            if(cropSlot.TryGetValue(request.grade, out int cropCount) == false)
                return ErrorPacket(ENetworkResult.Error);

            if(cropCount <= 0)
            {
                return new SellCropResponse() {
                    result = ENetworkResult.Success,
                    earnedGold = 0,
                    soldCropCount = 0
                };
            }

            StorageTableRow storageTableRow = DataTableManager.GetTable<StorageTable>().GetRowByLevel(userData.storageData.level);
            if(storageTableRow == null)
                return ErrorPacket(ENetworkResult.Error);

            CropTableRow cropTableRow = DataTableManager.GetTable<CropTable>().GetRowByProductID(request.id);
            if (cropTableRow == null)
                return ErrorPacket(ENetworkResult.Error);

            int earnedGold = (int)MathF.Ceiling(cropCount * cropTableRow.basePrice * storageTableRow.priceMultiplier);
            using (IRedLock userDataLock = await userDataInfo.LockAsync(redLockFactory))
            {
                cropSlot[request.grade] = 0;
                userData.monetaData.gold += earnedGold;

                await userDataInfo.WriteAsync();
            }

            return new SellCropResponse() {
                result = ENetworkResult.Success,
                earnedGold = earnedGold,
                soldCropCount = cropCount
            };
        }

        private static SellCropResponse ErrorPacket(ENetworkResult cause)
        {
            return new SellCropResponse() {
                result = cause,
            };
        }
    }
}