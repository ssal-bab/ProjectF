using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    public abstract class PacketProcessorBase<TReq, TRes> where TReq : RequestPacket where TRes : ResponsePacket
    {
        protected readonly DBManager dbManager = null;
        protected readonly IDistributedLockFactory redLockFactory;
        protected readonly TReq request = null;

        public TRes Response = null;
        public ENetworkResult Result = ENetworkResult.None;

        public PacketProcessorBase(DBManager dbManager, IDistributedLockFactory redLockFactory, TReq request)
        {
            this.dbManager = dbManager;
            this.redLockFactory = redLockFactory;
            this.request = request;
        }

        public async Task ProcessAsync()
        {
            Response = await ProcessInternal();
            Result = Response == null ? ENetworkResult.Error : Response.result;
        }

        protected abstract Task<TRes> ProcessInternal();
    }
}