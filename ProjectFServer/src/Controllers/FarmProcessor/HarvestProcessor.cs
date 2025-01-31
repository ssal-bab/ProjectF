using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public class HarvestProcessor : PacketProcessorBase<HarvestRequest, HarvestResponse>
    {
        public HarvestProcessor(DBManager dbManager, IDistributedLockFactory redLockFactory, HarvestRequest request) : base(dbManager, redLockFactory, request) { }

        protected override async Task<HarvestResponse> ProcessInternal()
        {
            return new HarvestResponse() {
                result = ENetworkResult.Success,
                productCropID = 0
            };
        }
    }
}