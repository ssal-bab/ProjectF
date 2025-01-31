using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class PlantProcessor : PacketProcessorBase<PlantRequest, PlantResponse>
    {
        public PlantProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, PlantRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<PlantResponse> ProcessInternal()
        {
            return new PlantResponse() {
                result = ENetworkResult.Success,
                cropID = request.cropID
            };
        }
    }
}