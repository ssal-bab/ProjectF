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
            return new PlantCropResponse() {
                result = ENetworkResult.Success,
                cropID = request.cropID
            };
        }
    }
}