using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            int earnedGold = new CalculateCropPrice(request.id, cropCount, userData.storageData.level).cropPrice;
            if(earnedGold == -1)
                return ErrorPacket(ENetworkResult.DataNotFound);

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
    }
}