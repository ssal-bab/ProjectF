using Microsoft.AspNetCore.Mvc;
using ProjectF.Networks.DataBases;
using ProjectF.Networks.Packets;
using RedLockNet;

namespace ProjectF.Networks.Controllers
{
    [ApiController]
    public abstract class PacketControllerBase : ControllerBase
    {
        protected readonly DBManager dbManager = null;
        protected readonly IDistributedLockFactory redLockFactory;

        public PacketControllerBase(DBManager dbManager, IDistributedLockFactory redLockFactory)
        {
            this.dbManager = dbManager;
            this.redLockFactory = redLockFactory;
        }
    }
}